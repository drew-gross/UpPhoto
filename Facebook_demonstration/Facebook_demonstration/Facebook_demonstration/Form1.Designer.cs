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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.myWallTextBox = new System.Windows.Forms.TextBox();
            this.uidTextBox = new System.Windows.Forms.TextBox();
            this.friendWallTextBox = new System.Windows.Forms.TextBox();
            this.screenShotButton = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.uidToAddTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.FaceboxWatcher = new System.IO.FileSystemWatcher();
            ((System.ComponentModel.ISupportInitialize)(this.FaceboxWatcher)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(21, 30);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 39);
            this.button1.TabIndex = 0;
            this.button1.Text = "Publish to my wall";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.PublishToMyWall_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(21, 111);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 82);
            this.button2.TabIndex = 1;
            this.button2.Text = "Publish to a friend\'s wall";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.PublishToFriendWall_Click);
            // 
            // myWallTextBox
            // 
            this.myWallTextBox.Location = new System.Drawing.Point(116, 46);
            this.myWallTextBox.Name = "myWallTextBox";
            this.myWallTextBox.Size = new System.Drawing.Size(149, 20);
            this.myWallTextBox.TabIndex = 2;
            // 
            // uidTextBox
            // 
            this.uidTextBox.Location = new System.Drawing.Point(116, 127);
            this.uidTextBox.Name = "uidTextBox";
            this.uidTextBox.Size = new System.Drawing.Size(149, 20);
            this.uidTextBox.TabIndex = 3;
            // 
            // friendWallTextBox
            // 
            this.friendWallTextBox.Location = new System.Drawing.Point(116, 173);
            this.friendWallTextBox.Name = "friendWallTextBox";
            this.friendWallTextBox.Size = new System.Drawing.Size(149, 20);
            this.friendWallTextBox.TabIndex = 4;
            // 
            // screenShotButton
            // 
            this.screenShotButton.Location = new System.Drawing.Point(21, 208);
            this.screenShotButton.Name = "screenShotButton";
            this.screenShotButton.Size = new System.Drawing.Size(75, 62);
            this.screenShotButton.TabIndex = 5;
            this.screenShotButton.Text = "Take a screen shot and upload it";
            this.screenShotButton.UseVisualStyleBackColor = true;
            this.screenShotButton.Click += new System.EventHandler(this.screenShotButton_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(21, 288);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 55);
            this.button3.TabIndex = 6;
            this.button3.Text = "Request friendship";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.AddFriend_Click);
            // 
            // uidToAddTextBox
            // 
            this.uidToAddTextBox.Location = new System.Drawing.Point(116, 304);
            this.uidToAddTextBox.Name = "uidToAddTextBox";
            this.uidToAddTextBox.Size = new System.Drawing.Size(149, 20);
            this.uidToAddTextBox.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(113, 288);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Friend uid :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(113, 111);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Friend uid :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(113, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Message :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(113, 157);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Message :";
            // 
            // FaceboxWatcher
            // 
            this.FaceboxWatcher.EnableRaisingEvents = true;
            this.FaceboxWatcher.IncludeSubdirectories = true;
            this.FaceboxWatcher.Path = "C:\\TestFolder";
            this.FaceboxWatcher.SynchronizingObject = this;
            this.FaceboxWatcher.Changed += new System.IO.FileSystemEventHandler(this.FaceboxWatcher_Changed);
            this.FaceboxWatcher.Created += new System.IO.FileSystemEventHandler(this.FaceboxWatcher_Created);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(277, 368);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.uidToAddTextBox);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.screenShotButton);
            this.Controls.Add(this.friendWallTextBox);
            this.Controls.Add(this.uidTextBox);
            this.Controls.Add(this.myWallTextBox);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.FaceboxWatcher)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox myWallTextBox;
        private System.Windows.Forms.TextBox uidTextBox;
        private System.Windows.Forms.TextBox friendWallTextBox;
        private System.Windows.Forms.Button screenShotButton;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox uidToAddTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        public System.IO.FileSystemWatcher FaceboxWatcher;
    }
}

