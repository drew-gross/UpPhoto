namespace FacebookApplication
{
    partial class MainWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.UpPhotoTrayMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ChangeAccountItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LogoutItem = new System.Windows.Forms.ToolStripMenuItem();
            this.WatchFolderItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExitItem = new System.Windows.Forms.ToolStripMenuItem();
            this.UpPhotoIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.UpPhotoTrayMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // UpPhotoTrayMenu
            // 
            this.UpPhotoTrayMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ChangeAccountItem,
            this.LogoutItem,
            this.WatchFolderItem,
            this.ExitItem});
            this.UpPhotoTrayMenu.Name = "UpPhotoTrayMenu";
            this.UpPhotoTrayMenu.Size = new System.Drawing.Size(164, 114);
            // 
            // ChangeAccountItem
            // 
            this.ChangeAccountItem.Name = "ChangeAccountItem";
            this.ChangeAccountItem.Size = new System.Drawing.Size(163, 22);
            this.ChangeAccountItem.Text = "Change Account";
            this.ChangeAccountItem.Click += new System.EventHandler(this.ChangeAccountItem_Click);
            // 
            // LogoutItem
            // 
            this.LogoutItem.Name = "LogoutItem";
            this.LogoutItem.Size = new System.Drawing.Size(163, 22);
            this.LogoutItem.Text = "Logout";
            this.LogoutItem.Click += new System.EventHandler(this.LogoutItem_Click);
            // 
            // WatchFolderItem
            // 
            this.WatchFolderItem.Name = "WatchFolderItem";
            this.WatchFolderItem.Size = new System.Drawing.Size(163, 22);
            this.WatchFolderItem.Text = "Watch Folders";
            this.WatchFolderItem.Click += new System.EventHandler(this.WatchFolderItem_Click);
            // 
            // ExitItem
            // 
            this.ExitItem.Name = "ExitItem";
            this.ExitItem.Size = new System.Drawing.Size(163, 22);
            this.ExitItem.Text = "Exit";
            // 
            // UpPhotoIcon
            // 
            this.UpPhotoIcon.BalloonTipText = "UpPhoto";
            this.UpPhotoIcon.ContextMenuStrip = this.UpPhotoTrayMenu;
            this.UpPhotoIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("UpPhotoIcon.Icon")));
            this.UpPhotoIcon.Text = "UpPhoto";
            this.UpPhotoIcon.Visible = true;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 264);
            this.Name = "MainWindow";
            this.ShowInTaskbar = false;
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.UpPhotoTrayMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip UpPhotoTrayMenu;
        private System.Windows.Forms.NotifyIcon UpPhotoIcon;
        private System.Windows.Forms.ToolStripMenuItem ChangeAccountItem;
        private System.Windows.Forms.ToolStripMenuItem LogoutItem;
        private System.Windows.Forms.ToolStripMenuItem WatchFolderItem;
        private System.Windows.Forms.ToolStripMenuItem ExitItem;
    }
}

