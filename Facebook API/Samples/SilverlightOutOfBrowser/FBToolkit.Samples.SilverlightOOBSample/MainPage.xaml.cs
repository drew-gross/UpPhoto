using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.IO;
using System.IO.IsolatedStorage;
using System.Xml;
using System.Xml.Linq;
using Facebook.Schema;
using Facebook.Session;
using Facebook.Utility;
using Facebook.BindingHelper;
using System.ComponentModel;

namespace FBToolkit.Samples.SilverlightOOBSample
{
    public partial class MainPage : UserControl
    {
        FacebookSession _session;

        const string SessionKey = "SessionKey";
        const string SessionSecret = "SessionSecret";
        const string CacheFileName = "cache.xml";
        const string ApplicationKey = "42a5950c30dbf980804f9e173d05c37b";
        BindingManager _bind;

        public MainPage()
        {
            InitializeComponent();

            if (Application.Current.IsRunningOutOfBrowser)
            {
                string sessionKey, sessionSecret;

                Install.Visibility = Visibility.Collapsed;
                ReadCachedData(out sessionKey, out sessionSecret);
                if (sessionKey == null)
                {
                    Login.Visibility = Visibility.Collapsed;
                    OOBLogin.Visibility = Visibility.Visible;
                }
                _session = new CachedSession(ApplicationKey, sessionKey, sessionSecret);
            }
            else
            {
                Install.Visibility = Visibility.Collapsed;
                _session = new BrowserSession(ApplicationKey, new Enums.ExtendedPermissions[]{Enums.ExtendedPermissions.offline_access});
            }

            _session.LoginCompleted += new EventHandler<AsyncCompletedEventArgs>(session_LoginCompleted);
        }

        void session_LoginCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                Deployment.Current.Dispatcher.BeginInvoke(new Action(() => Login.Visibility = Visibility.Collapsed));
                Deployment.Current.Dispatcher.BeginInvoke(new Action(() => Install.Visibility = Application.Current.InstallState == InstallState.Installed ? Visibility.Collapsed : Visibility.Visible));
                Deployment.Current.Dispatcher.BeginInvoke(new Action(() => BindObjects()));
            }
            else
            {
                Deployment.Current.Dispatcher.BeginInvoke(new Action(() => Login.Visibility = Application.Current.InstallState == InstallState.Installed ? Visibility.Collapsed : Visibility.Visible));
                Deployment.Current.Dispatcher.BeginInvoke(new Action(() => OOBLogin.Content = Application.Current.InstallState == InstallState.Installed ? "Launch Browser" : "Login"));
                Deployment.Current.Dispatcher.BeginInvoke(new Action(() => OOBLogin.Visibility = Application.Current.InstallState == InstallState.Installed ? Visibility.Visible : Visibility.Collapsed));
            }
        }

        void BindObjects()
        {
            _bind = BindingManager.CreateInstance(_session);
            Friends.ItemsSource = _bind.Friends;
            FriendsAlbums.ItemsSource = _bind.FriendsAlbums;
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            _session.Login();
        }
        private void OOBLogin_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Install_Click(object sender, RoutedEventArgs e)
        {
            SaveSessionKey();
            Application.Current.Install();
            Deployment.Current.Dispatcher.BeginInvoke(new Action(() => Install.Visibility = Visibility.Collapsed));
        }

        void ReadCachedData(out string sessionkey, out string sessionSecret)
        {
            sessionkey = null;
            sessionSecret = null;
            string data = null;

            try
            {
                using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (IsolatedStorageFileStream isfs = new IsolatedStorageFileStream(CacheFileName, FileMode.Open, isf))
                    {
                        using (StreamReader sr = new StreamReader(isfs))
                        {
                            string lineOfData = String.Empty;
                            while ((lineOfData = sr.ReadLine()) != null)
                                data += lineOfData;
                        }

                        string[] tokens = data.Split('$');
                        sessionkey = tokens[0];
                        sessionSecret = tokens[1];
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        void SaveSessionKey()
        {
            string data = string.Format("{0}${1}", _session.SessionKey, _session.SessionSecret);

            try
            {
                using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (IsolatedStorageFileStream isfs = isf.OpenFile(CacheFileName, FileMode.Create))
                    {
                        using (StreamWriter sw = new StreamWriter(isfs))
                        {
                            sw.Write(data);
                            sw.Close();
                        }
                    }
                }
            }
            catch (Exception e)
            {
            }
        }
    }
}
