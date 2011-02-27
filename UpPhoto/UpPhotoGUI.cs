using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace UpPhoto
{
    public class UpPhotoGUI : Control
    {
        private System.ComponentModel.IContainer components = new Container();
        ComponentResourceManager trayIcons = new ComponentResourceManager(typeof(SystemTrayIcons));

        MainWindow parent;

        ContextMenuStrip UpPhotoTrayMenu;

        ToolStripMenuItem ChangeAccountItem = new ToolStripMenuItem();
        ToolStripMenuItem LogoutItem = new ToolStripMenuItem();
        ToolStripMenuItem WatchFolderItem = new ToolStripMenuItem();
            ToolStripMenuItem AddWatchedFolderItem = new ToolStripMenuItem();
        ToolStripMenuItem ExitItem = new ToolStripMenuItem();

        NotifyIcon UpPhotoIcon;

        Dictionary<WatchedFolder, ToolStripMenuItem> menuItemMap = new Dictionary<WatchedFolder, ToolStripMenuItem>();

        public UpPhotoGUI(MainWindow newParent)
        {
            Application.EnableVisualStyles();

            parent = newParent;

            UpPhotoTrayMenu = new ContextMenuStrip(components);
            UpPhotoTrayMenu.SuspendLayout();

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

            UpPhotoIcon = new NotifyIcon(components);
            UpPhotoIcon.BalloonTipText = "UpPhoto";
            UpPhotoIcon.ContextMenuStrip = UpPhotoTrayMenu;
            UpPhotoIcon.Text = "UpPhoto";
            UpPhotoIcon.Visible = true;
            UpPhotoIcon.Click += new System.EventHandler(TrayIcon_Click);

            UpPhotoTrayMenu.ResumeLayout(false);
        }

        protected override void Dispose(bool disposing)
        {
             if (disposing && (components != null))
             {
                 components.Dispose();
             }
             base.Dispose(disposing);
        }

        public void SetTrayIcon(String iconPath)
        {
            var newIcon = trayIcons.GetObject(iconPath);
            UpPhotoIcon.Icon = ((System.Drawing.Icon)(newIcon));
        }

        public void AddWatchedFolder(WatchedFolder newFolder)
        {
            ToolStripMenuItem menuItem = new ToolStripMenuItem(newFolder.Path());

            ToolStripMenuItem UnwatchItem = new ToolStripMenuItem("Unwatch");
            UnwatchItem.Click += new EventHandler(newFolder.UnwatchItem_Click);
            menuItem.DropDownItems.Add(UnwatchItem);

            ToolStripMenuItem ViewItem = new ToolStripMenuItem("View");
            ViewItem.Click += new EventHandler(newFolder.ViewItem_Click);
            menuItem.DropDownItems.Add(ViewItem);

            WatchFolderItem.DropDownItems.Add(menuItem);
            menuItemMap[newFolder] = menuItem;
        }

        public void RemoveWatchedFolder(WatchedFolder oldFolder)
        {
            WatchFolderItem.DropDownItems.Remove(menuItemMap[oldFolder]);
            menuItemMap.Remove(oldFolder);
        }

        private void ExitItem_Click(object sender, EventArgs e)
        {
            parent.updateHandler.StopThreads();
            parent.SaveData();
            this.Dispose(true);
            Application.Exit();
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
                parent.WatchFolder(dialog.SelectedPath);
            }
        }

        private void TrayIcon_Click(object sender, EventArgs e)
        {
            
        }
    }
}
