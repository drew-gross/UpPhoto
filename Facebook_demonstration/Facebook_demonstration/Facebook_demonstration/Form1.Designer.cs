namespace Facebook_demonstration
{
    partial class Form1
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
            this.screenShotButton = new System.Windows.Forms.Button();
            this.FaceboxWatcher = new System.IO.FileSystemWatcher();
            ((System.ComponentModel.ISupportInitialize)(this.FaceboxWatcher)).BeginInit();
            this.SuspendLayout();
            // 
            // screenShotButton
            // 
            this.screenShotButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.screenShotButton.Location = new System.Drawing.Point(0, 0);
            this.screenShotButton.Name = "screenShotButton";
            this.screenShotButton.Size = new System.Drawing.Size(162, 62);
            this.screenShotButton.TabIndex = 5;
            this.screenShotButton.Text = "Upload Photo";
            this.screenShotButton.UseVisualStyleBackColor = true;
            this.screenShotButton.Click += new System.EventHandler(this.screenShotButton_Click);
            // 
            // FaceboxWatcher
            // 
            this.FaceboxWatcher.EnableRaisingEvents = true;
            this.FaceboxWatcher.IncludeSubdirectories = true;
            this.FaceboxWatcher.Path = "C:\\TestFolder";
            this.FaceboxWatcher.SynchronizingObject = this;
            this.FaceboxWatcher.Changed += new System.IO.FileSystemEventHandler(this.FaceboxWatcher_Created);
            this.FaceboxWatcher.Created += new System.IO.FileSystemEventHandler(this.FaceboxWatcher_Created);
            this.FaceboxWatcher.Deleted += new System.IO.FileSystemEventHandler(this.FaceboxWatcher_Deleted);
            this.FaceboxWatcher.Renamed += new System.IO.RenamedEventHandler(this.FaceboxWatcher_Renamed);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(162, 62);
            this.Controls.Add(this.screenShotButton);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.FaceboxWatcher)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button screenShotButton;
        public System.IO.FileSystemWatcher FaceboxWatcher;
    }
}

