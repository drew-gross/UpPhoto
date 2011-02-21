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
            this.UpPhotoTrayMenu.SuspendLayout();
            this.SuspendLayout();
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
    }
}
