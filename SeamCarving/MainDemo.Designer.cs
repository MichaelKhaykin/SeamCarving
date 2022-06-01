namespace SeamCarving
{
    partial class ClockDemo
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.HorizontalCut = new System.Windows.Forms.Button();
            this.CutHorizontalSeam = new System.Windows.Forms.Button();
            this.mainbox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.mainbox)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1112, 51);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Cut";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1112, 93);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Seam";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 60;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // HorizontalCut
            // 
            this.HorizontalCut.Location = new System.Drawing.Point(1099, 142);
            this.HorizontalCut.Name = "HorizontalCut";
            this.HorizontalCut.Size = new System.Drawing.Size(108, 93);
            this.HorizontalCut.TabIndex = 3;
            this.HorizontalCut.Text = "Cut Horizontal";
            this.HorizontalCut.UseVisualStyleBackColor = true;
            this.HorizontalCut.Click += new System.EventHandler(this.HorizontalCut_Click);
            // 
            // CutHorizontalSeam
            // 
            this.CutHorizontalSeam.Location = new System.Drawing.Point(1099, 254);
            this.CutHorizontalSeam.Name = "CutHorizontalSeam";
            this.CutHorizontalSeam.Size = new System.Drawing.Size(108, 93);
            this.CutHorizontalSeam.TabIndex = 4;
            this.CutHorizontalSeam.Text = "Horizontal Seam";
            this.CutHorizontalSeam.UseVisualStyleBackColor = true;
            this.CutHorizontalSeam.Click += new System.EventHandler(this.CutHorizontalSeam_Click);
            // 
            // mainbox
            // 
            this.mainbox.Location = new System.Drawing.Point(12, 12);
            this.mainbox.Name = "mainbox";
            this.mainbox.Size = new System.Drawing.Size(253, 199);
            this.mainbox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.mainbox.TabIndex = 0;
            this.mainbox.TabStop = false;
            // 
            // ClockDemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1388, 616);
            this.Controls.Add(this.CutHorizontalSeam);
            this.Controls.Add(this.HorizontalCut);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.mainbox);
            this.Name = "ClockDemo";
            this.Text = "MainDemo";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.mainbox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.PictureBox mainbox;
        private System.Windows.Forms.Button HorizontalCut;
        private System.Windows.Forms.Button CutHorizontalSeam;
    }
}

