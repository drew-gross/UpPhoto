namespace WinformsSample
{
	partial class FriendViewer
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.profile1 = new Facebook.Winforms.Profile();
            this.friendList1 = new Facebook.Winforms.FriendList();
            this.photoAlbum1 = new Facebook.Winforms.PhotoAlbum();
            this.facebookService1 = new Facebook.Winforms.Components.FacebookService(this.components);
            this.SuspendLayout();
            // 
            // profile1
            // 
            this.profile1.BackColor = System.Drawing.Color.White;
            this.profile1.Location = new System.Drawing.Point(12, 12);
            this.profile1.Name = "profile1";
            this.profile1.Size = new System.Drawing.Size(347, 317);
            this.profile1.TabIndex = 0;
            this.profile1.User = null;
            // 
            // friendList1
            // 
            this.friendList1.AutoScroll = true;
            this.friendList1.BackColor = System.Drawing.Color.White;
            this.friendList1.Friends = null;
            this.friendList1.Location = new System.Drawing.Point(299, 1);
            this.friendList1.Name = "friendList1";
            this.friendList1.Size = new System.Drawing.Size(376, 123);
            this.friendList1.TabIndex = 1;
            // 
            // photoAlbum1
            // 
            this.photoAlbum1.Albums = null;
            this.photoAlbum1.BackColor = System.Drawing.Color.White;
            this.photoAlbum1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.photoAlbum1.Location = new System.Drawing.Point(365, 130);
            this.photoAlbum1.Name = "photoAlbum1";
            this.photoAlbum1.Size = new System.Drawing.Size(310, 264);
            this.photoAlbum1.TabIndex = 2;
            // 
            // facebookService1
            // 
            this.facebookService1.ApplicationKey = null;
            this.facebookService1.SessionKey = null;
            this.facebookService1.uid = ((long)(0));
            // 
            // FriendViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(686, 429);
            this.Controls.Add(this.photoAlbum1);
            this.Controls.Add(this.friendList1);
            this.Controls.Add(this.profile1);
            this.Name = "FriendViewer";
            this.Text = "TestService";
            this.Load += new System.EventHandler(this.TestService_Load);
            this.ResumeLayout(false);

		}

		#endregion

		private Facebook.Winforms.Profile profile1;
        private Facebook.Winforms.FriendList friendList1;
		private Facebook.Winforms.PhotoAlbum photoAlbum1;
        private Facebook.Winforms.Components.FacebookService facebookService1;
	}
}