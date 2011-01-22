using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Facebook.Rest;
using Facebook.Schema;
using Facebook.Utility;
using Facebook.Winforms.Components;
using FacebookController;
using SHDocVw;

namespace Facebook_demonstration
{
    public partial class Form1 : Form
    {
        #region Class members
        readonly FacebookService fbService = new FacebookService();

        private ScreenShotFormatter screenShotFormatter;

        private List<long> friendsUids;

        private List<Point> friendsPositions;
        #endregion

        public Form1()
        {
            InitializeComponent();

            // The application key of the Facebook application used
            fbService.ApplicationKey = "";

            // Add all needed permissions
            List<Enums.ExtendedPermissions> perms = new List<Enums.ExtendedPermissions>
                                                    {
                                                                        Enums.ExtendedPermissions.none
                                                                    };
            fbService.ConnectToFacebook(perms);            
        }

        #region Button Handlers
        private void PublishToFriendWall_Click(object sender, EventArgs e)
        {
            PublishToAFriendWall();
        }

        private void PublishToMyWall_Click(object sender, EventArgs e)
        {
            PublishToMyWall();
        }

        private void screenShotButton_Click(object sender, EventArgs e)
        {
            PublishPhotos();
        }

        private void AddFriend_Click(object sender, EventArgs e)
        {
            AddNewFriend();
        }
        #endregion

        #region Facebook Interfaces
        public void PublishToAFriendWall()
        {
            try
            {
                attachment att = new attachment
                {
                    // Name of link
                    name = "",
                    // URL of link
                    href = "",
                    caption = "",
                    media = new List<attachment_media>()
                };

                attachment_media_image attMEd = new attachment_media_image
                {
                    // Image source
                    src = "",
                    // URL to go to if clicked
                    href = ""
                };
                att.media.Add(attMEd);

                action_link a = new action_link
                {
                    text = "What's this",
                    //URL to go to if clicked
                    href = ""
                };
                IList<action_link> tempA = new List<action_link> { a };

                // Use the typed friend UID to publish the typed message
                fbService.Stream.PublishAsync(friendWallTextBox.Text, att, tempA, uidTextBox.Text, 0, PublishAsyncCompleted, null);
            }
            catch (Exception)
            {

            }
        }

        private static void PublishAsyncCompleted(string result, Object state, FacebookException e)
        {

        }

        public void PublishToMyWall()
        {
            try
            {
                attachment att = new attachment
                {
                    name = "",
                    href = "",
                    caption = "has used the application",
                    media = new List<attachment_media>()
                };

                attachment_media_image attMEd = new attachment_media_image
                {
                    src = "",
                    href = ""
                };
                att.media.Add(attMEd);

                action_link a = new action_link
                {
                    text = "",
                    href = ""
                };
                IList<action_link> tempA = new List<action_link> { a };

                fbService.Stream.PublishAsync(myWallTextBox.Text, att, tempA, null, 0, PublishAsyncCompleted, null);
            }
            catch (Exception)
            {

            }
        }

        public bool PublishPhotos()
        {
            // Just for demonstration////////////////////////////////////////////////

            // The origin of the screen shot
            Point origin = new Point(0,0);

            // The size of it
            Point size = new Point(800,600);

            /////////////////////////////////////////////////////////////////////////

            // Used for the tagging feature//////////////////////////////////////////

            // Uids of friends to be tagged
            friendsUids = new List<long>();

            // Corresponding positions
            //Note: Positions are in terms of percentage relative to screen shot size 
            friendsPositions = new List<Point>();

            /////////////////////////////////////////////////////////////////////////
            
            try
            {
                screenShotFormatter = new ScreenShotFormatter(origin.X, origin.Y, size.X, size.Y);

                IList<album> albums = fbService.Photos.GetAlbums();

                string albumAid = "";
                foreach (album album in albums)
                {
                    // Album name to create - if doesn't exist - is "Trial Album"
                    if (album.name == "Trial Album")
                    {
                        albumAid = album.aid;
                        break;
                    }
                }

                // If not found, create it
                if (albumAid == "")
                {
                    fbService.Photos.CreateAlbumAsync("Trial Album", null, "Album description", CreateAlbumCallback, null);
                    return true;
                }

                fbService.Photos.UploadAsync(albumAid,
                    "A photo to remember.",
                    screenShotFormatter.
                        GetScreenShot(),
                    Enums.FileType.png,
                    UploadCallback,
                    null);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void CreateAlbumCallback(album album, object state, FacebookException e)
        {
            fbService.Photos.UploadAsync(album.aid,
                "A photo to remember.",
                screenShotFormatter.GetScreenShot(),
                Enums.FileType.png,
                UploadCallback,
                null);
        }

        private void UploadCallback(photo p, object state, FacebookException e)
        {
            if (friendsUids != null && friendsPositions != null)
                PhotoTagger(p.pid);
        }

        private void PhotoTagger(string photoPid)
        {
            for (int i = 0; i < friendsUids.Count; i++)
            {
                fbService.Photos.AddTag(photoPid, friendsUids[i], null, friendsPositions[i].X, friendsPositions[i].Y);
            }
        }

        public void AddNewFriend()
        {
            object o = null;
            InternetExplorer ie = new InternetExplorerClass();

            IWebBrowserApp wb = ie;
            wb.Visible = true;
            wb.ToolBar = 0;
            wb.Width = 600;
            wb.Height = 600;
            wb.Navigate("http://www.facebook.com/addfriend.php?id=" + uidToAddTextBox.Text, ref o, ref o, ref o, ref o);
        }
        #endregion
    }
}
