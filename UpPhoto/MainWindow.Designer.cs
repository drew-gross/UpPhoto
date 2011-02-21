namespace UpPhoto
{
    partial class MainWindow
    {

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 264);
            this.Name = "MainWindow";
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.UpPhotoTrayMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon UpPhotoIcon;
        private System.Windows.Forms.ToolStripMenuItem ChangeAccountItem;
        private System.Windows.Forms.ToolStripMenuItem LogoutItem;
        private System.Windows.Forms.ToolStripMenuItem WatchFolderItem;
        private System.Windows.Forms.ToolStripMenuItem ExitItem;
        private System.Windows.Forms.ContextMenuStrip UpPhotoTrayMenu;
        private System.Windows.Forms.ToolStripMenuItem AddWatchedFolderItem;
    }
}

