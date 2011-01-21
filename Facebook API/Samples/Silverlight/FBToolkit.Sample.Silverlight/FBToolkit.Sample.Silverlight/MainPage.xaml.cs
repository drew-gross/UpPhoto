using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Diagnostics;
using System.Windows.Browser;
using Facebook.Rest;
using Facebook.Schema;
using Facebook.Session;
using Facebook.Utility;

namespace FBToolkit.Sample.Silverlight
{
    public partial class MainPage
    {
        #region Private Members

        private Api _fb;
        readonly BrowserSession _browserSession;
        StreamStoryCollection streamCollection;
        private session_info _session;
        private string _token;
        private const string ApplicationKey = "c13ad2db575481d4736cfa218ce54141";
        private const string ApplicationSecret = "3cf797b3532f9db29993ae92a90d5bff";
        
        #endregion Private Members

        #region Constructor

        public MainPage()
        {
            InitializeComponent();
            _browserSession = new BrowserSession(ApplicationKey);
            _browserSession.LoginCompleted += browserSession_LoginCompleted;

            // Disable tabs until login completes
            ToggleTabStatus(false, new List<TabItem> { LoginTab });
            ToggleLoginStatus(false);
        }

        #endregion Constructor

        #region Auth

        private void ToggleTabStatus(bool isEnabled, List<TabItem> remainEnabledTabs)
        {
            if(remainEnabledTabs == null) remainEnabledTabs = new List<TabItem>();

            foreach(var t in tabContainer.Items)
            {
                var tabItem = t as TabItem;
                if(tabItem == null) continue;

                tabItem.IsEnabled = (remainEnabledTabs.Contains(tabItem)) || isEnabled;
            }
        }

        private void ToggleLoginStatus(bool isEnabled)
        {
            CreateToken.IsEnabled = isEnabled;
            Browser.IsEnabled = isEnabled;
            GetSession.IsEnabled = isEnabled;
        }

        void browserSession_LoginCompleted(object sender, EventArgs e)
        {
            _fb = new Api(_browserSession);
            //_facebookAPI.Friends.GetAsync(new Friends.GetFriendsCallback(GetFriendsComplete), null);

            // Enable tabs until login completes
            ToggleTabStatus(true, null);
            ToggleLoginStatus(true);
        }
        
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            _browserSession.Login();
        }

        private void CreateToken_Click(object sender, RoutedEventArgs e)
        {
            _fb.Auth.CreateTokenAsync(OnCreateTokenCompleted, null);
        }
        
        private void Browser_Click(object sender, RoutedEventArgs e)
        {
            const string facebookLoginUrl = "https://login.facebook.com/login.php?api_key={0}&auth_token={1}&v=1.0&popup";
            Uri loginurl = new Uri(String.Format(facebookLoginUrl, _browserSession.ApplicationKey, _token));
            HtmlPage.Window.Navigate(loginurl, "__blank");

        }

        private void GetSession_Click(object sender, RoutedEventArgs e)
        {
            _fb.Auth.GetSessionAsync(_token, OnGetSessionCompleted, null);
        }

        private void OnCreateTokenCompleted(string token, Object state, FacebookException e)
        {
            if (e == null)
            {
                Dispatcher.BeginInvoke(() =>
                                           {
                                               MessageBox.Show(string.Format("Token created as {0}.", token));
                                               _token = token;                               
                                           });
            }
            else
            {
                Dispatcher.BeginInvoke(() => MessageBox.Show("Error: " + e.Message));
            }
        }

        private void OnGetSessionCompleted(session_info session, Object state, FacebookException e)
        {
            if (e == null)
            {
                Dispatcher.BeginInvoke(() =>
                                           {
                                               MessageBox.Show(string.Format("Session Key created as {0}.",
                                                                             session.session_key));
                                               _session = session;
                                           });
            }
            else
            {
                Dispatcher.BeginInvoke(() => MessageBox.Show("Error: " + e.Message));
            }
        }

        #endregion

        #region Friends

        private void FriendsButton_Click(object sender, RoutedEventArgs e)
        {
            _fb.Friends.GetAsync(new Friends.GetFriendsCallback(GetFriendsCompleted), null);
        }
        
        private void FriendsDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            _fb.Friends.GetUserObjectsAsync(new Users.GetInfoCallback(GetFriendsInfoCompleted), null);
        }

        void GetFriendsCompleted(IList<long> uids, Object state, FacebookException e)
        {
            if (e == null)
            {
                Dispatcher.BeginInvoke(() => FriendsId.ItemsSource = uids);
            }
            else
            {
                Dispatcher.BeginInvoke(() => MessageBox.Show("Error: " + e.Message));
            }
        }

        void GetFriendsInfoCompleted(IList<user> users, Object state, FacebookException e)
        {
            if (e == null)
            {
                Dispatcher.BeginInvoke(() => Friends.ItemsSource = users);
            }
            else
            {
                Dispatcher.BeginInvoke(() => MessageBox.Show("Error: " + e.Message));
            }
        }

        #endregion

        #region Profiles

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            _fb.Users.GetInfoAsync(new Users.GetInfoCallback(GetUserInfoCompleted), null);
        }

        void GetUserInfoCompleted(IList<user> users, Object state, FacebookException e)
        {
            if (e == null)
            {
                user u = users.First();
                if (u.pic != null)
                {
                    Uri uri = new Uri(u.pic);
                    Dispatcher.BeginInvoke(() =>
                    {
                        ProfilePhoto.Source = new BitmapImage(uri);
                        ProfileStatus.Text = u.status.message;
                        ProfileName.Text = u.first_name + " " + u.last_name;
                    });
                }
            }
            else
            {
                Dispatcher.BeginInvoke(() => MessageBox.Show("Error: " + e));
            }
        }

        #endregion

        #region Albums
        private void GetAlbums_Click(object sender, RoutedEventArgs e)
        {
            _fb.Photos.GetAlbumsAsync(null, new Photos.GetAlbumsCallback(OnGetAlbums), null);
        }

        private void GetPhotos_Click(object sender, RoutedEventArgs e)
        {
            if (AlbumList.SelectedIndex != -1)
            {
                album al = (album)AlbumList.SelectedItem;
                _fb.Photos.GetAsync(null, al.aid, null, OnGetPhotos, null);

            }
        }

        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog { Multiselect = false };
            openfile.ShowDialog();

            System.IO.Stream fileStream = openfile.File.OpenRead();
            byte[] data;
            using (BinaryReader reader = new BinaryReader(fileStream))
            {
                data = reader.ReadBytes((int)fileStream.Length);
            }
            fileStream.Close();

            _fb.Photos.UploadAsync(null, "Myphoto", data, "image/jpeg", OnUploadPhoto, null);
        }

        void OnGetAlbums(IList<album> albums, Object state, FacebookException e)
        {
            if (e == null)
            {
                Dispatcher.BeginInvoke(() => AlbumList.ItemsSource = albums);
            }
            else
            {
                Dispatcher.BeginInvoke(() => MessageBox.Show("Error: " + e.Message));
            }

        }

        void OnGetPhotos(IList<photo> photos, Object state, FacebookException e)
        {
            if (e == null)
            {
                Dispatcher.BeginInvoke(() => PhotoList.ItemsSource = photos);
            }
            else
            {
                Dispatcher.BeginInvoke(() => MessageBox.Show("Error: " + e.Message));
            }

        }

        private static void OnUploadPhoto(photo p, Object state, FacebookException e)
        {
            if (e == null)
            {
            }
            else
            {
                Console.WriteLine(e);
            }
        }

        #endregion

        #region Status

        private void SetStatusButton_Click(object sender, RoutedEventArgs e)
        {
            _fb.Status.SetAsync(StatusText.Text, SetStatusCompleted, null);
        }

        private void GetStatusButton_Click(object sender, RoutedEventArgs e)
        {
            _fb.Status.GetAsync(new Status.GetCallback(GetStatusCompleted), null);
        }

        void SetStatusCompleted(bool result, Object state, FacebookException e)
        {
            if (e == null)
            {
                if (result == false)
                {
                    Dispatcher.BeginInvoke(() => MessageBox.Show("call failed"));
                }
            }
            else
            {
                Dispatcher.BeginInvoke(() => MessageBox.Show("Error: " + e.Message));
            }
        }

        void GetStatusCompleted(IList<user_status> status, Object state, FacebookException e)
        {
            if (e == null)
            {
                Dispatcher.BeginInvoke(() => StatusList.ItemsSource = status);
            }
            else
            {
                Dispatcher.BeginInvoke(() => MessageBox.Show("Error: " + e.Message));
            }
        }

        #endregion

        #region Extended Properties

        private void GetExtendedPermission_Click(object sender, RoutedEventArgs e)
        {
            Enums.ExtendedPermissions perm;

            if (Status_update.IsChecked != null && Status_update.IsChecked.Value)
            {
                perm = Enums.ExtendedPermissions.status_update;
            }
            else if (Create_Event.IsChecked != null && Create_Event.IsChecked.Value)
            {
                perm = Enums.ExtendedPermissions.create_event;
            }
            else if (photo_upload.IsChecked != null && photo_upload.IsChecked.Value)
            {
                perm = Enums.ExtendedPermissions.create_event;
            }
            else if (publish_stream.IsChecked != null && publish_stream.IsChecked.Value)
            {
                perm = Enums.ExtendedPermissions.publish_stream;
            }
            else if (read_stream.IsChecked != null && read_stream.IsChecked.Value)
            {
                perm = Enums.ExtendedPermissions.read_stream;
            }
            else
            {
                MessageBox.Show("Should select a permission to get permission");
                return;
            }

            Uri uri = GetExtendedPermissionUrl(perm);
            HtmlPage.Window.Navigate(uri, "__blank");
        }

        private void CheckExtendedPermission_Click(object sender, RoutedEventArgs e)
        {
            Enums.ExtendedPermissions perm;

            if (Status_update.IsChecked != null && Status_update.IsChecked.Value)
            {
                perm = Enums.ExtendedPermissions.status_update;
            }
            else if (Create_Event.IsChecked != null && Create_Event.IsChecked.Value)
            {
                perm = Enums.ExtendedPermissions.create_event;
            }
            else if (photo_upload.IsChecked != null && photo_upload.IsChecked.Value)
            {
                perm = Enums.ExtendedPermissions.create_event;
            }
            else if (publish_stream.IsChecked != null && publish_stream.IsChecked.Value)
            {
                perm = Enums.ExtendedPermissions.publish_stream;
            }
            else if (read_stream.IsChecked != null && read_stream.IsChecked.Value)
            {
                perm = Enums.ExtendedPermissions.read_stream;
            }
            else
            {
                MessageBox.Show("Should select a permission to get permission");
                return;
            }

            Uri uri = GetExtendedPermissionUrl(perm);
            HtmlPage.Window.Navigate(uri, "__blank");
        }

        #endregion Extended Properties

        #region Stream

        private void GetStream_Click(object sender, RoutedEventArgs e)
        {
            _fb.Stream.GetAsync(null, DateTime.Now - TimeSpan.FromDays(20), null, null, OnGetStream, null);
        }

        private void GetFilters_Click(object sender, RoutedEventArgs e)
        {
            _fb.Stream.GetFiltersAsync(OnGetStreamFilters, null);
        }

        void OnGetStream(stream_data data, Object state, FacebookException e)
        {
            if (e == null)
            {
                Dispatcher.BeginInvoke(() => UpdateStreamUi(data));
            }
            else
            {
                Dispatcher.BeginInvoke(() => MessageBox.Show("Error: " + e.Message));
            }
        }

        void OnGetStreamFilters(IList<stream_filter> filters, Object state, FacebookException e)
        {
            if (e == null)
            {
                foreach (stream_filter filter in filters)
                {
                    Debug.WriteLine(filter.name);
                }
            }
            else
            {
                Dispatcher.BeginInvoke(() => MessageBox.Show("Error: " + e.Message));
            }
        }

        private void PublishStream_Click(object sender, RoutedEventArgs e)
        {
            attachment attachment = new attachment
                                        {
                name = "Title you see on page",
                description = "Description of text",
                caption = "Caption string",
                href = "http://msn.com"
            };

            action_link al = new action_link
                                 {
                text = "Action text",
                href = "http://msn.com",
            };
            //FacebookStreamActionLink[] als = new[] { al };
            IList<action_link> als = new List<action_link> {al};
            _fb.Stream.PublishAsync("Test message", attachment, als,  null, 0, OnStreamPublish, null);
        }

        void OnStreamPublish(string id, Object state, FacebookException e)
        {
            if (e == null)
            {
                Dispatcher.BeginInvoke(() => MessageBox.Show("Id: " + id));
            }
            else
            {
                Dispatcher.BeginInvoke(() => MessageBox.Show("Error: " + e.Message));
            }
        }

        void UpdateStreamUi(stream_data data)
        {
            streamCollection = new StreamStoryCollection();
            Dictionary<long, profile> profileDict = new Dictionary<long, profile>();

            foreach (profile p in data.profiles.profile)
            {
                profileDict.Add(p.id, p);
            }

            // TODO: Implement stream iteration
            //foreach (stream_post post in data.posts.stream_post)
            //{
            //    if (!profileDict.ContainsKey(post.actor_id))
            //    {
            //        continue;
            //    }

            //    StreamStory sstory = new StreamStory(post, profileDict[post.actor_id]);

            //    if (post.comments.comment_list != null)
            //    {
            //        foreach (FacebookComment c in post.comments.)
            //        {
            //            if (!profileDict.ContainsKey(c.FromId))
            //            {
            //                continue;
            //            }

            //            sstory.comments.Add(new StreamStoryComment(c, profileDict[c.FromId]));
            //        }
            //    }

            //    streamCollection.Add(sstory);
            //}

            StreamList.ItemsSource = streamCollection;
        }

        #endregion
        
        #region Event Handlers

        private static Uri GetExtendedPermissionUrl(Enums.ExtendedPermissions permission)
        {
            return new Uri(String.Format("http://www.facebook.com/authorize.php?api_key={0}&v=1.0&ext_perm={1}", ApplicationKey, permission));
        }

        #endregion
        
        #region Events

        private void CreateEvent_Click(object sender, RoutedEventArgs e)
        {
            facebookevent evt = new facebookevent
                                    {
                                        start_time =
                                            DateHelper.ConvertDateToFacebookDate(new DateTime(2009, 6, 11, 11, 0, 0)),
                                        end_time =
                                            DateHelper.ConvertDateToFacebookDate(new DateTime(2009, 6, 11, 12, 0, 0)),
                                        name = "Test Event",
                                        description = "SOme description",
                                        event_type = "Party",
                                        event_subtype = "Birthday Party",
                                        location = "My house",
                                        host = "Me"
                                    };

            _fb.Events.CreateAsync(evt, OnCreateEvent, null);

        }

        private void GetEvents_Click(object sender, RoutedEventArgs e)
        {
            _fb.Events.GetAsync(null, null, null, null, null, OnGetEvents, null);
        }

        private void GetMembers_Click(object sender, RoutedEventArgs e)
        {
            if (EventsList.SelectedIndex != -1)
            {
                facebookevent ev = (facebookevent)EventsList.SelectedItem;
                _fb.Events.GetMembersAsync(ev.eid, OnGetMembers, null);
            }
        }

        void OnCreateEvent(long eid, Object o, FacebookException e)
        {
            if (e == null)
            {
                Dispatcher.BeginInvoke(() => MessageBox.Show("Eventid: " + eid));
            }
            else
            {
                Dispatcher.BeginInvoke(() => MessageBox.Show("Error: " + e.Message));
            }
        }

        void OnGetEvents(IList<facebookevent> events, Object o, FacebookException e)
        {
            if (e == null)
            {
                Dispatcher.BeginInvoke(() => EventsList.ItemsSource = events);
            }
            else
            {
                Dispatcher.BeginInvoke(() => MessageBox.Show("Error: " + e.Message));
            }
        }

        void OnGetMembers(event_members members, Object o, FacebookException e)
        {
            if (e == null)
            {
                Dispatcher.BeginInvoke(() =>
                                           {
                                               List<string> responses = new List<string>();
                                               foreach (var member in members.attending.uid)
                                               {
                                                   responses.Add(string.Format("{0} (attending)", member));
                                               }
                                               foreach (var member in members.declined.uid)
                                               {
                                                   responses.Add(string.Format("{0} (declined)", member));
                                               }
                                               foreach (var member in members.not_replied.uid)
                                               {
                                                   responses.Add(string.Format("{0} (no reply)", member));
                                               }
                                               foreach (var member in members.unsure.uid)
                                               {
                                                   responses.Add(string.Format("{0} (unsure)", member));
                                               }

                                               EventsMembersList.ItemsSource = responses;
                                           });
            }
            else
            {
                Dispatcher.BeginInvoke(() => MessageBox.Show("Error: " + e.Message));
            }
        }
        #endregion
    }
}