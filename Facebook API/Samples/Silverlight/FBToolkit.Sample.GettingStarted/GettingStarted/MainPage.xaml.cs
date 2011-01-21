using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Facebook.Schema;

namespace GettingStarted
{
    public partial class MainPage
    {
        // Active instance of Data Access object
        private readonly FacebookDataAccess _dataAccess;

        // Dummy timer instance
        private readonly DispatcherTimer _dummyTimer;
        
        #region Methods

        #region Constructor 

        /// <summary>
        /// Constructor.
        /// </summary>
        public MainPage()
        {
            // Init user control
            InitializeComponent();

            // Set initial auth display
            SetAuthenticationStatus();

            // New up data access instance and wire up auth/initialize event handlers
            _dataAccess = new FacebookDataAccess();
            _dataAccess.LoginCompleted += DataAccess_LoginCompleted;
            _dataAccess.LogoutCompleted += DataAccess_LogoutCompleted;
            _dataAccess.InitializeCompleted += DataAccess_InitializeCompleted;

            // Create a dummy timer to increment and show UI is responsive
            _dummyTimer = new DispatcherTimer();
            _dummyTimer.Tick += _dummyTimer_Tick;
            _dummyTimer.Interval = new TimeSpan(0, 0, 1);
            _dummyTimer.Start();
        }

        #endregion Constructor

        #region Private Methods

        // Toggle enabled status by authentication state
        private void SetAuthenticationStatus()
        {
            LoginButton.IsEnabled = !IsAuthenticated();
            LogoutButton.IsEnabled = IsAuthenticated();
            RefreshButton.IsEnabled = IsAuthenticated();
        }

        // Determine Facebook Authentication status
        private bool IsAuthenticated()
        {
            return _dataAccess != null && _dataAccess.Session != null && _dataAccess.Session.UserId > 0;
        }

        // Configure for Data Access and notifications
        private void Initialize()
        {
            // Assign user control's data context to instance of data access object
            DataContext = _dataAccess;

            // Create delegate handler to update album caption when album collection changes
            _dataAccess.UserAlbums.CollectionChanged += delegate
            {
                // Set album count
                UserAlbumCaption.Text = string.Format("User Albums: {0}", _dataAccess.UserAlbums.Count);
            };
        }
        
        #endregion Private Methods

        #region Event Handlers

        // Event handler listens for final initialization notification from data access object
        private void DataAccess_InitializeCompleted(object sender, EventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(Initialize);
        }

        // Event handler listens for facebook logout notification from data access object
        private void DataAccess_LogoutCompleted(object sender, EventArgs e)
        {
            // Reset auth button bar
            SetAuthenticationStatus();    
        }

        // Event handler listens for facebook login notification from data access object
        private void DataAccess_LoginCompleted(object sender, EventArgs e)
        {
            // Reset auth button bar
            SetAuthenticationStatus();
        }

        // Login button click event handler
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            // Issue login request to data access object
            _dataAccess.Login();
        }

        // Logout button click event handler
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            // Issue logout request to data access object
            _dataAccess.Logout();
        }
        
        // Current Album selection changed event
        private void AlbumList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Clear any album photo captions
            AlbumPhotoCaption.Text = string.Empty;
            AlbumPhotoCacheCaption.Text = string.Empty;

            // Retrieve listbox that issued the event
            var listBox = sender as ListBox;
            if (listBox != null && listBox.SelectedItem != null)
            {
                // Set data access' current album to the newly selected item (as album)
                _dataAccess.CurrentAlbum = listBox.SelectedItem as album;

                // Set cache loaded and async loaded album photo counts
                AlbumPhotoCacheCaption.Text = string.Format("Original Load Album Photos: {0}", _dataAccess.AlbumPhotos.Count);
                AlbumPhotoCaption.Text = string.Format("Total Album Photos: {0}", _dataAccess.AlbumPhotos.Count);

                // Create delegate handler to update album photo caption when album photo collection changes
                _dataAccess.AlbumPhotos.CollectionChanged += delegate
                {
                    // Set album photo total count
                    AlbumPhotoCaption.Text = string.Format("Total Album Photos: {0}", _dataAccess.AlbumPhotos.Count);
                };
            }
        }

        // Refresh button click event handler
        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            // Force a refresh of bound objects (albums and current album photos)
            _dataAccess.RefreshItemCollections();
        }

        private void _dummyTimer_Tick(object sender, EventArgs e)
        {
            DummyTimer.Text = DateTime.Now.ToLongTimeString();
        }

        #endregion Event Handlers

        #endregion Methods
    }
}
