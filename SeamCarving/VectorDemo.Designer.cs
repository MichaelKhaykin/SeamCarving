namespace SeamCarving
{
    partial class VectorDemo
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
            this.mainbox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.mainbox)).BeginInit();
            this.SuspendLayout();
            // 
            // mainbox
            // 
            this.mainbox.Image = global::SeamCarving.Properties.Resources.smallMario;
            this.mainbox.Location = new System.Drawing.Point(12, 12);
            this.mainbox.Name = "mainbox";
            this.mainbox.Size = new System.Drawing.Size(100, 116);
            this.mainbox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.mainbox.TabIndex = 0;
            this.mainbox.TabStop = false;
            // 
            // VectorDemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1278, 586);
            this.Controls.Add(this.mainbox);
            this.Name = "VectorDemo";
            this.Text = "VectorDemo";
            this.Load += new System.EventHandler(this.VectorDemo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.mainbox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox mainbox;
    }
}