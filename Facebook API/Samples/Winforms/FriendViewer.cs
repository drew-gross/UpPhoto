using System;
using System.Windows.Forms;
using Facebook.Schema;
using Facebook.Winforms;
using FBToolkit.Samples.Winforms.Properties;
using System.Collections.Generic;

namespace WinformsSample
{
	public partial class FriendViewer : Form
	{
		public FriendViewer()
		{
			InitializeComponent();
			facebookService1.ApplicationKey = Settings.Default.api_key;
            facebookService1.ConnectToFacebook(new List<Enums.ExtendedPermissions>() { Enums.ExtendedPermissions.read_stream, Enums.ExtendedPermissions.publish_stream});
		}

		private void TestService_Load(object sender, EventArgs e)
		{
			ListenToEvents(true);
			try
			{
				var friends = facebookService1.Friends.GetUserObjects();
				var me = facebookService1.Users.GetInfo();
				LoadUserBasedControls(me);
				friendList1.Friends = friends;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				Close();
			}
		}

		private void ListenToEvents(bool listen)
		{
			if (listen)
			{
				friendList1.FriendSelected += friendList1_FriendSelected;
			}
		}

		private void friendList1_FriendSelected(object sender, FriendSelectedEventArgs e)
		{
			LoadUserBasedControls(e.User);
		}

		private void LoadUserBasedControls(user user)
		{
			profile1.User = user;
		    photoAlbum1.Albums = facebookService1.Photos.GetAlbums(user.uid.Value);
		}
	}
}