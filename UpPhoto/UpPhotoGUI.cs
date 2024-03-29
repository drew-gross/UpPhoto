﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Reflection;
using System.Diagnostics;
using System.Drawing;

namespace UpPhoto
{
    public class UpPhotoGUI : Control
    {
        private System.ComponentModel.IContainer components = new Container();
        ComponentResourceManager trayIcons = new ComponentResourceManager(typeof(SystemTrayIcons));

        MainWindow parent;

        ContextMenuStrip UpPhotoTrayMenu;

        ToolStripMenuItem AboutItem = new ToolStripMenuItem();
        ToolStripMenuItem ViewItem = new ToolStripMenuItem();
        ToolStripMenuItem ExitItem = new ToolStripMenuItem();
        public ToolStripMenuItem DownloadPhotosItem = new ToolStripMenuItem();

        public NotifyIcon UpPhotoIcon;

        Dictionary<WatchedFolder, ToolStripMenuItem> menuItemMap = new Dictionary<WatchedFolder, ToolStripMenuItem>();

        public UpPhotoGUI(MainWindow newParent)
        {
            Application.EnableVisualStyles();

            parent = newParent;

            UpPhotoTrayMenu = new ContextMenuStrip(components);
            UpPhotoTrayMenu.SuspendLayout();

            UpPhotoTrayMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] 
            {
                AboutItem,
                ViewItem,
                ExitItem,
                DownloadPhotosItem
            });

            AboutItem.Text = "About UpPhoto";
            AboutItem.Click += (x, y) => { Process.Start(@"http://www.upphoto.ca/instructions.php"); };

            ViewItem.Text = "View UpPhoto folder";
            ViewItem.Click += ViewItem_Click;

            ExitItem.Text = "Exit";
            ExitItem.Click += ExitItem_Click;

            DownloadPhotosItem.Text = "Download my photos from Facebook";
            DownloadPhotosItem.CheckOnClick = true;
            DownloadPhotosItem.Checked = false;

            UpPhotoIcon = new NotifyIcon(components);
            UpPhotoIcon.ContextMenuStrip = UpPhotoTrayMenu;
            UpPhotoIcon.Text = "UpPhoto";
            UpPhotoIcon.Visible = true;
            UpPhotoIcon.MouseClick += TrayIcon_Click;
            UpPhotoIcon.DoubleClick += ViewItem_Click;

            UpPhotoIcon.BalloonTipTitle = "Thank you for using UpPhoto!";
            UpPhotoIcon.BalloonTipText = "Click on this icon to view your UpPhoto folder. Put photos in the folder, and they will be uploaded to Facebook.";

            UpPhotoTrayMenu.ResumeLayout(true);
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
            try
            {
                if (trayIcons == null || iconPath == null)
                {
                    return;
                }
                Icon newIcon = (System.Drawing.Icon)trayIcons.GetObject(iconPath);
                if (UpPhotoIcon == null || newIcon == null)
                {
                    return;
                }
                UpPhotoIcon.Icon = newIcon;
            }
            catch (System.Exception)
            {
            	//wtf? why is this empty. should at least have a comment if its supposed to be empty
            }
        }

        private void ExitItem_Click(object sender, EventArgs e)
        {
            Dispose(true);
            MainWindow.QuitUpPhoto();
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

        private void TrayIcon_Click(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //Allows left click to open menu
                MethodInfo mi = typeof(NotifyIcon).GetMethod("ShowContextMenu", BindingFlags.Instance | BindingFlags.NonPublic);
                mi.Invoke(UpPhotoIcon, null);
            }
        }

        public void ViewItem_Click(object sender, EventArgs e)
        {
            String path = parent.WatchedFolderPaths()[0];
            Process.Start(path);
        }
    }
}
