using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace UpPhoto
{
    public class UpPhotoGUI : Form
    {
        private System.ComponentModel.IContainer components = new Container();
        ComponentResourceManager trayIcons = new ComponentResourceManager(typeof(SystemTrayIcons));
        const String IdleIconPath = "Idle";
        const String UploadingIconPath = "Uploading";
        const String DownloadingIconPath = "Downloading";
        const String UploadingAndDownloadingIconPath = "UploadingAndDownloading";
        const String NotConnectedIconPath = "NotConnected";

        MainWindow parent;

        ContextMenuStrip UpPhotoTrayMenu;

        ToolStripMenuItem ChangeAccountItem = new ToolStripMenuItem();
        ToolStripMenuItem LogoutItem = new ToolStripMenuItem();
        ToolStripMenuItem WatchFolderItem = new ToolStripMenuItem();
            ToolStripMenuItem AddWatchedFolderItem = new ToolStripMenuItem();
        ToolStripMenuItem ExitItem = new ToolStripMenuItem();

        NotifyIcon UpPhotoIcon;

        public UpPhotoGUI(MainWindow newParent)
        {
            parent = newParent;
            UpPhotoIcon = new NotifyIcon(components);
            UpPhotoTrayMenu.SuspendLayout();
            SuspendLayout();

            UpPhotoTrayMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] 
            {
                ChangeAccountItem,
                LogoutItem,
                WatchFolderItem,
                ExitItem
            });

            ChangeAccountItem.Text = "Change Account";
            ChangeAccountItem.Click += new System.EventHandler(ChangeAccountItem_Click);

            LogoutItem.Text = "Logout";
            LogoutItem.Click += new System.EventHandler(LogoutItem_Click);

            WatchFolderItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] 
            {
                AddWatchedFolderItem
            });
            WatchFolderItem.Text = "Watch Folders";

            AddWatchedFolderItem.Text = "Add";
            AddWatchedFolderItem.Click += new System.EventHandler(AddWatchedFolderItem_Click);

            ExitItem.Text = "Exit";
            ExitItem.Click += new System.EventHandler(ExitItem_Click);

            UpPhotoIcon.BalloonTipText = "UpPhoto";
            UpPhotoIcon.ContextMenuStrip = UpPhotoTrayMenu;
            UpPhotoIcon.Icon = ((System.Drawing.Icon)(trayIcons.GetObject(NotConnectedIconPath)));
            UpPhotoIcon.Text = "UpPhoto";
            UpPhotoIcon.Visible = true;
            UpPhotoIcon.Click += new System.EventHandler(TrayIcon_Click);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void ExitItem_Click(object sender, EventArgs e)
        {
            parent.updateHandler.StopThreads();
            parent.SaveData();
            Close();
        }

        private void ChangeAccountItem_Click(object sender, EventArgs e)
        {
            // Logout, then show prompt again.
            throw new NotImplementedException();
        }

        private void LogoutItem_Click(object sender, EventArgs e)
        {
            FacebookInterfaces.LogOut();
        }

        private void AddWatchedFolderItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                WatchFolder(dialog.SelectedPath);
            }
        }

        private void TrayIcon_Click(object sender, EventArgs e)
        {
            //SetForegroundWindow(new HandleRef(this, this.Handle));
            //UpPhotoTrayMenu.Show(this, this.PointToClient(Cursor.Position));
        }
    }
}
