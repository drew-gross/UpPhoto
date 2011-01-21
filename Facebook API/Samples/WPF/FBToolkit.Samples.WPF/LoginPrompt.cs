using System;
using System.Collections.Generic;
using System.Text;
using Standard;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using System.Windows.Navigation;
using Contigo;

namespace NewsFeedSample
{
    public class LoginPrompt
    {
        private FacebookService _service;
        private Permissions _requestedPermissions;
        private string _authToken;

        public LoginPrompt(FacebookService service, Permissions requestedPermissions)
        {
            Debug.Assert(service != null);
            if (service.IsLoggedIn)
            {
                throw new ArgumentException("The provided service is already logged in.");
            }
            _service = service;
            _requestedPermissions = requestedPermissions;
            _authToken = _service.GenerateAuthenticationToken();
        }

        private IEnumerable<Uri> _GetPermissionUris()
        {
            Permissions pendingRequests = _service.GetMissingPermissions(_requestedPermissions);
            if (pendingRequests == Permissions.None)
            {
                yield break;
            }

            for (int i = 1, bitshift = 0; bitshift < 32; i <<= 1, ++bitshift)
            {
                var currentPermission = (Permissions)i;
                if ((pendingRequests & currentPermission) != Permissions.None)
                {
                    yield return _service.GetExtendedPermissionsUri(currentPermission);
                }
            }
        }

        public bool ShowDialog()
        {
            // This is clunky from a user standpoint, but Facebook doesn't
            // seem to give a decent way make this workflow more seamless...

            using (var loginBrowser = new WebBrowser())
            {
                var window = new Window
                {
                    Content = loginBrowser,
                    Title = "Login to Facebook",
                };


                loginBrowser.Navigate(_service.GetLoginUri(_authToken));

                loginBrowser.Navigated += (sender, e) =>
                {
                    // This will be contained in the page once the user has accepted the app.
                    if (e.Uri.PathAndQuery.Contains("desktopapp.php"))
                    {
                        // Start the session without the cache.  This object is just spinning up to get a valid session token.
                        string sessionKey, userId;
                        _service.InitiateSession(_authToken, out sessionKey, out userId);
                        _service.RecoverSession(sessionKey, userId);

                        window.Close();
                    }
                    else if (e.Uri.PathAndQuery.Contains(_service.AppId) || e.Uri.PathAndQuery.Contains("tos.php"))
                    {
                        // Keep in this browser as long as it appears that we're in the context of this app.
                        return;
                    }
                    else
                    {
                        // User did something other than log into the application.
                        // Spawn a new webpage with the nevigated URI and close this browser session.
                        Process.Start(e.Uri.ToString());
                        window.Close();
                    }
                };

                window.ShowDialog();
            }

            if (!_service.IsLoggedIn)
            {
                // User canceled the dialog.
                return false;
            }

            bool madeRequests = false;
            // Display a separate window for each permission request.
            foreach (Uri uri in _GetPermissionUris())
            {
                madeRequests = true;
                using (var permissionBrowser = new WebBrowser())
                {
                    var permissionWindow = new Window
                    {
                        Content = permissionBrowser,
                        Title = "Grant access to Facebook information.",
                    };

                    permissionBrowser.Navigate(uri);
                    // We have to let the user dismiss the window...
                    permissionWindow.ShowDialog();
                }
            }

            // If at the end of the cascade of Windows there are still outstanding permission requests, return false.
            if (madeRequests && _service.GetMissingPermissions(_requestedPermissions) != Permissions.None)
            {
                return false;
            }

            return true;
        }
    }
}
