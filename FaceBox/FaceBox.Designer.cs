namespace FaceBox
{
    partial class FaceBox
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.FaceBoxWatcher = new System.IO.FileSystemWatcher();
            ((System.ComponentModel.ISupportInitialize)(this.FaceBoxWatcher)).BeginInit();
            // 
            // FaceBoxWatcher
            // 
            this.FaceBoxWatcher.EnableRaisingEvents = true;
            this.FaceBoxWatcher.IncludeSubdirectories = true;
            this.FaceBoxWatcher.Path = "C:\\Users\\Drew Gross\\Documents\\Projects\\Facebook Hackathon\\TestFaceBox";
            this.FaceBoxWatcher.Changed += new System.IO.FileSystemEventHandler(this.FaceBoxWatcher_Changed);
            // 
            // FaceBox
            // 
            this.ServiceName = "FaceBox";
            ((System.ComponentModel.ISupportInitialize)(this.FaceBoxWatcher)).EndInit();

        }

        #endregion

        private System.IO.FileSystemWatcher FaceBoxWatcher;

    }
}
