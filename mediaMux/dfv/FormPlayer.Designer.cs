namespace df
{
    partial class FormPlayer
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.steptonextframeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copytimestapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.makeselectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cancelselectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonPause = new System.Windows.Forms.Button();
            this.labelTime = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.imageCroppingBox1 = new df.ImageCroppingBox();
            this.myProgressBar1 = new df.MyProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackColor = System.Drawing.Color.Black;
            this.pictureBox1.ContextMenuStrip = this.contextMenuStrip1;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(960, 540);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseClick);
            this.pictureBox1.Resize += new System.EventHandler(this.pictureBox1_Resize);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.steptonextframeToolStripMenuItem,
            this.copytimestapToolStripMenuItem,
            this.makeselectionToolStripMenuItem,
            this.cancelselectionToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(200, 100);
            // 
            // steptonextframeToolStripMenuItem
            // 
            this.steptonextframeToolStripMenuItem.Name = "steptonextframeToolStripMenuItem";
            this.steptonextframeToolStripMenuItem.Size = new System.Drawing.Size(199, 24);
            this.steptonextframeToolStripMenuItem.Text = "next_frame";
            this.steptonextframeToolStripMenuItem.Click += new System.EventHandler(this.steptonextframeToolStripMenuItem_Click);
            // 
            // copytimestapToolStripMenuItem
            // 
            this.copytimestapToolStripMenuItem.Name = "copytimestapToolStripMenuItem";
            this.copytimestapToolStripMenuItem.Size = new System.Drawing.Size(199, 24);
            this.copytimestapToolStripMenuItem.Text = "copy_timestamp";
            this.copytimestapToolStripMenuItem.Click += new System.EventHandler(this.copytimestapToolStripMenuItem_Click);
            // 
            // makeselectionToolStripMenuItem
            // 
            this.makeselectionToolStripMenuItem.Name = "makeselectionToolStripMenuItem";
            this.makeselectionToolStripMenuItem.Size = new System.Drawing.Size(199, 24);
            this.makeselectionToolStripMenuItem.Text = "make_selection";
            this.makeselectionToolStripMenuItem.Click += new System.EventHandler(this.makeselectionToolStripMenuItem_Click);
            // 
            // cancelselectionToolStripMenuItem
            // 
            this.cancelselectionToolStripMenuItem.Name = "cancelselectionToolStripMenuItem";
            this.cancelselectionToolStripMenuItem.Size = new System.Drawing.Size(199, 24);
            this.cancelselectionToolStripMenuItem.Text = "cancel_selection";
            this.cancelselectionToolStripMenuItem.Click += new System.EventHandler(this.cancelselectionToolStripMenuItem_Click);
            // 
            // buttonPause
            // 
            this.buttonPause.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.buttonPause.Location = new System.Drawing.Point(0, 1);
            this.buttonPause.Name = "buttonPause";
            this.buttonPause.Size = new System.Drawing.Size(63, 31);
            this.buttonPause.TabIndex = 3;
            this.buttonPause.Text = "Pause";
            this.buttonPause.UseVisualStyleBackColor = true;
            this.buttonPause.Click += new System.EventHandler(this.buttonPause_Click);
            // 
            // labelTime
            // 
            this.labelTime.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelTime.AutoSize = true;
            this.labelTime.BackColor = System.Drawing.Color.White;
            this.labelTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.labelTime.Location = new System.Drawing.Point(77, 8);
            this.labelTime.Name = "labelTime";
            this.labelTime.Size = new System.Drawing.Size(55, 15);
            this.labelTime.TabIndex = 5;
            this.labelTime.Text = "label1";
            // 
            // timer1
            // 
            this.timer1.Interval = 200;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.buttonPause);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.labelTime);
            this.panel1.Controls.Add(this.richTextBox1);
            this.panel1.Controls.Add(this.myProgressBar1);
            this.panel1.Location = new System.Drawing.Point(4, 542);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(960, 59);
            this.panel1.TabIndex = 6;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(868, 31);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(84, 28);
            this.button1.TabIndex = 8;
            this.button1.Text = "Ok";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Location = new System.Drawing.Point(0, 34);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(862, 25);
            this.richTextBox1.TabIndex = 7;
            this.richTextBox1.Text = "text";
            // 
            // timer2
            // 
            this.timer2.Interval = 10;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // imageCroppingBox1
            // 
            this.imageCroppingBox1.BackColor = System.Drawing.Color.Black;
            this.imageCroppingBox1.ContextMenuStrip = this.contextMenuStrip1;
            this.imageCroppingBox1.Image = null;
            this.imageCroppingBox1.IsLockSelected = false;
            this.imageCroppingBox1.IsSetClip = true;
            this.imageCroppingBox1.Location = new System.Drawing.Point(68, 52);
            this.imageCroppingBox1.MaskColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.imageCroppingBox1.Name = "imageCroppingBox1";
            this.imageCroppingBox1.SelectedRectangle = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.imageCroppingBox1.Size = new System.Drawing.Size(755, 366);
            this.imageCroppingBox1.TabIndex = 7;
            this.imageCroppingBox1.Text = "imageCroppingBox1";
            this.imageCroppingBox1.Visible = false;
            // 
            // myProgressBar1
            // 
            this.myProgressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.myProgressBar1.BackColor = System.Drawing.Color.White;
            this.myProgressBar1.Location = new System.Drawing.Point(64, 2);
            this.myProgressBar1.Maximum = 1000;
            this.myProgressBar1.Name = "myProgressBar1";
            this.myProgressBar1.Size = new System.Drawing.Size(888, 29);
            this.myProgressBar1.TabIndex = 4;
            // 
            // FormPlayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(960, 603);
            this.Controls.Add(this.imageCroppingBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pictureBox1);
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(678, 442);
            this.Name = "FormPlayer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormPlayer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormPlayer_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormPlayer_FormClosed);
            this.Load += new System.EventHandler(this.FormPlayer_Load);
            this.ResizeBegin += new System.EventHandler(this.FormPlayer_ResizeBegin);
            this.ResizeEnd += new System.EventHandler(this.FormPlayer_ResizeEnd);
            this.SizeChanged += new System.EventHandler(this.FormPlayer_SizeChanged);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FormPlayer_KeyUp);
            this.Move += new System.EventHandler(this.FormPlayer_Move);
            this.Resize += new System.EventHandler(this.FormPlayer_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button buttonPause;
        private df.MyProgressBar myProgressBar1;
        private System.Windows.Forms.Label labelTime;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem steptonextframeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copytimestapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem makeselectionToolStripMenuItem;
        private ImageCroppingBox imageCroppingBox1;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.ToolStripMenuItem cancelselectionToolStripMenuItem;
    }
}