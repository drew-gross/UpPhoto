using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using Facebook.Rest;
using Facebook.Schema;
using Facebook.Session;
using Facebook.Utility;

namespace GettingStarted
{
    /// <summary>
    /// FacebookDataAccess object handles all interaction with Facebook Developer Toolkit
    /// and Facebook authentication mechanism.
    /// </summary>
    public class FacebookDataAccess : INotifyPropertyChanged
    {
        #region Events

        // INotifyPropertyChanged event
        public event PropertyChangedEventHandler PropertyChanged;

        // Authentication and Initialization events
        public event EventHandler LoginCompleted;
        public event EventHandler LogoutCompleted;
        public event EventHandler InitializeCompleted;

        #endregion Events

        #region Private Members

        private ObservableCollection<photo> _currentAlbumPhotos = new ObservableCollection<photo>();
        private ObservableCollection<album> _userAlbums = new ObservableCollection<album>();
        private readonly Dictionary<string, ObservableCollection<photo>> _albumPhotoHash = new Dictionary<string, ObservableCollection<photo>>();
        private readonly Dictionary<string, album> _albumHash = new Dictionary<string, album>();
        private album _currentAlbum;

        #endregion Private Members

        #region Properties

        /// <summary>
        /// Gets or sets the Facebook session object
        /// </summary>
        internal BrowserSession Session { get; set; }

        /// <summary>
        /// Gets or sets the Facebook API object, used for all Toolkit access to Facebook APIs
        /// </summary>
        internal Api API { get; set; }

        /// <summary>
        /// Gets or sets the authenticated Facebook user
        /// </summary>
        public user CurrentUser { get; set; }

        /// <summary>
        /// Gets or sets the collection of all user albums
        /// </summary>
        public ObservableCollection<album> UserAlbums
        {
            get
            {
                return _userAlbums;
            }
            set
            {
                _userAlbums = value;
                NotifyPropertyChanged("UserAlbums");
            }
        }

        /// <summary>
        /// Gets or sets the currently active Album.
        /// </summary>
        public album CurrentAlbum
        {
            get
            {
                return _currentAlbum;
            }
            set
            {
                _currentAlbum = value;

                // Initally return any existing photos for this album
                AlbumPhotos = GetCachedPhotosByAlbum();

                // Issue async request to retrieve album photos (adding new)
                GetAlbumPhotos();

                NotifyPropertyChanged("CurrentAlbum");
            }
        }

        /// <summary>
        /// Gets or sets the collection of the current album's photos
        /// </summary>
        public ObservableCollection<photo> AlbumPhotos
        {
            get
            {
                return _currentAlbumPhotos;
            }
            set
            {
                _currentAlbumPhotos = value;
                NotifyPropertyChanged("AlbumPhotos");
            }
        }

        #endregion Properties

        #region Methods

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public FacebookDataAccess()
        {
            CreateSession();
        }

        #endregion Constructor

        #region Public Methods

        /// <summary>
        /// Start requests to refresh each user object collection.
        /// </summary>
        public void RefreshItemCollections()
        {
            // Retrive user albums
            GetAlbumsByUser(CurrentUser.uid.Value);

            // Retrive current album's photos
            GetAlbumPhotos();
        }

        /// <summary>
        /// Retrieves the current user's album collection.
        /// </summary>
        public void GetAlbumsByUser(long userId)
        {
            // Issue async request for user albums via the Facebook Developer Toolkit
            API.Photos.GetAlbumsAsync(userId, GetUserAlbumsCompleted, UserAlbums);
        }

        /// <summary>
        /// Retrieves the current albums's photo collection.
        /// </summary>
        public void GetAlbumPhotos()
        {
            // Issue async request for all photos in the current album
            if (CurrentAlbum == null || CurrentAlbum.aid == null) return;
            API.Photos.GetAsync(null, CurrentAlbum.aid, null, GetAlbumPhotosCompleted, AlbumPhotos);
        }

        /// <summary>
        /// Issues Login request to Toolkit BrowserSession object.
        /// </summary>
        public void Login()
        {
            if (Session == null)
            {
                // New up BrowserSession 
                CreateSession();
            }

            // Request Facebook login via Facebook Developer Toolkit's BrowserSession object.
            if (Session != null) Session.Login();
        }

        /// <summary>
        /// Issues Logout request to Toolkit BrowserSession object.
        /// </summary>
        public void Logout()
        {
            if (Session != null)
            {
                // Request Facebook logout via Facebook Developer Toolkit's BrowserSession object.
                Session.Logout();
            }
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Retrieves the stored photos for the current album.
        /// </summary>
        private ObservableCollection<photo> GetCachedPhotosByAlbum()
        {
            // Return the existing cache of photos for current album
            if (_albumPhotoHash == null || CurrentAlbum == null) return null;
            return _albumPhotoHash[CurrentAlbum.aid];
        }

        /// <summary>
        /// Creates Facebook Developer Toolkit BrowserSession object used for Facebook Authencation.
        /// </summary>
        private void CreateSession()
        {
            // Create session object (would protect app key, app secret in live application)
            Session = new BrowserSession("239437ae3091963a74e28eaee9840f84");

            // Wire up authentication event handlers
            Session.LoginCompleted += browserSession_LoginCompleted;
            Session.LogoutCompleted += browserSession_LogoutCompleted;
        }

        /// <summary>
        /// Starts Facebook DataAccess async initialization sequence.
        /// </summary>
        private void Initialize()
        {
            // Refresh item collections (albums, photos)
            RefreshItemCollections();

            // Issue initialize completed event to subscribers
            OnInitializeCompleted(null);
        }
        
        #endregion Private Methods

        #region Async Response Handlers

        /// <summary>
        /// Handles GetUserInfo request result.
        /// </summary>
        private void GetUserInfoCompleted(IList<user> user, object sender, FacebookException e)
        {
            // Verify user returned
            if (user != null && user.Count > 0)
            {
                // Set current user
                CurrentUser = user[0];

                // Begin initialization of current user content
                Initialize();
                return;
            }

            // An error occurred, throw exception
            throw new Exception("Unable to load current user information.");
        }

        /// <summary>
        /// Handles GetAlbumsByUser request result.
        /// </summary>
        /// <param name="albums"></param>
        /// <param name="state"></param>
        /// <param name="exception"></param>
        private void GetUserAlbumsCompleted(IList<album> albums, object state, FacebookException exception)
        {
            // Marshall back to UI thread
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                // Verify albums returned
                if (albums == null) return;

                // If stateful (existing) collection is null, new up collection
                ObservableCollection<album> statefulAlbums = state as ObservableCollection<album> ?? new ObservableCollection<album>();

                // Iterate result set
                foreach (var a in albums)
                {
                    // Check for existing album in the album list cache
                    if (!_albumHash.ContainsKey(a.aid))
                    {
                        // Album is new, add to collection, album list cache and new up photo collection for this album
                        statefulAlbums.Add(a);
                        _albumHash.Add(a.aid, a);
                        _albumPhotoHash.Add(a.aid, new ObservableCollection<photo>());
                    }
                }
            });
        }

        /// <summary>
        /// Handles GetAlbumPhotos aysnc request result.
        /// </summary>
        private static void GetAlbumPhotosCompleted(IList<photo> photos, object state, FacebookException exception)
        {
            // Fake a longer delay
            System.Threading.Thread.Sleep(4000);

            // Marshall back to UI thread
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                // Verify photos returned
                if (photos == null) return;

                // If stateful (existing) collection is null, new up collection
                ObservableCollection<photo> statefulPhotos = state as ObservableCollection<photo> ?? new ObservableCollection<photo>();

                // Iterate result set
                foreach (var p in photos)
                {
                    // Set flag to determine existence
                    var photoExistsInCollection = false;

                    // Iterate existing photo cache
                    foreach (var existingPhoto in statefulPhotos)
                    {
                        // Check for matching photo IDs
                        if (existingPhoto.pid == p.pid)
                        {
                            // This is a duplicate, ignore and break
                            photoExistsInCollection = true;
                            break;
                        }
                    }

                    // Check if photo does not exist in cache
                    if (!photoExistsInCollection)
                    {
                        // Add to photo collection
                        statefulPhotos.Add(p);
                    }
                }
            });
        }

        #endregion Async Response Handlers

        #region Event Handlers

        /// <summary>
        /// BrowserSession Toolkit object Login completed event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void browserSession_LoginCompleted(object sender, AsyncCompletedEventArgs e)
        {
            // Create instance of Toolkit API using our authenticated session
            API = new Api(Session);

            // Load user information asynchronously
            API.Users.GetInfoAsync(Session.UserId, GetUserInfoCompleted, null);

            // Issue login event to subscribers
            OnLoginCompleted(e);
        }

        /// <summary>
        /// BrowserSession Toolkit object Logout completed event handler.
        /// </summary>
        private void browserSession_LogoutCompleted(object sender, AsyncCompletedEventArgs e)
        {
            // Clear session-related objects
            Session = null;
            API = null;

            // Issue logout event to subscribers
            OnLogoutCompleted(e);
        }

        #endregion Event Handlers

        #region Event Publishers

        private void OnLoginCompleted(EventArgs e)
        {
            if (LoginCompleted != null)
            {
                LoginCompleted(this, e);
            }
        }

        private void OnLogoutCompleted(EventArgs e)
        {
            if (LogoutCompleted != null)
            {
                LogoutCompleted(this, e);
            }
        }

        private void OnInitializeCompleted(EventArgs e)
        {
            if (InitializeCompleted != null)
            {
                InitializeCompleted(this, e);
            }
        }

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion Event Publishers

        #endregion Methods
    }
}
