namespace SeamCarving
{
    partial class KernelVisualization
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
            this.mainBox = new System.Windows.Forms.PictureBox();
            this.UpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.mainBox)).BeginInit();
            this.SuspendLayout();
            // 
            // mainBox
            // 
            this.mainBox.Location = new System.Drawing.Point(1, 0);
            this.mainBox.Name = "mainBox";
            this.mainBox.Size = new System.Drawing.Size(1151, 545);
            this.mainBox.TabIndex = 0;
            this.mainBox.TabStop = false;
            this.mainBox.Click += new System.EventHandler(this.mainBox_Click);
            // 
            // UpdateTimer
            // 
            this.UpdateTimer.Enabled = true;
            this.UpdateTimer.Interval = 50;
            this.UpdateTimer.Tick += new System.EventHandler(this.UpdateTimer_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(291, 102);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 26);
            this.label1.TabIndex = 1;
            this.label1.Text = "label1";
            // 
            // KernelVisualization
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1158, 546);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.mainBox);
            this.Name = "KernelVisualization";
            this.Text = "KernelVisualization";
            this.Load += new System.EventHandler(this.KernelVisualization_Load);
            ((System.ComponentModel.ISupportInitialize)(this.mainBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox mainBox;
        private System.Windows.Forms.Timer UpdateTimer;
        private System.Windows.Forms.Label label1;
    }
}