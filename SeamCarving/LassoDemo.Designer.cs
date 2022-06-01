namespace SeamCarving
{
    partial class LassoDemo
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
            this.mainbox = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.lassoButton = new System.Windows.Forms.Button();
            this.loop = new System.Windows.Forms.Timer(this.components);
            this.raiseEnergyLevelButtons = new System.Windows.Forms.Button();
            this.lowerEnergyLevelsButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.mainbox)).BeginInit();
            this.SuspendLayout();
            // 
            // mainbox
            // 
            this.mainbox.Image = global::SeamCarving.Properties.Resources.starrynight;
            this.mainbox.Location = new System.Drawing.Point(2, 1);
            this.mainbox.Name = "mainbox";
            this.mainbox.Size = new System.Drawing.Size(400, 317);
            this.mainbox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.mainbox.TabIndex = 1;
            this.mainbox.TabStop = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1253, 6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Cut";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1253, 35);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "Seam";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // lassoButton
            // 
            this.lassoButton.Location = new System.Drawing.Point(1253, 65);
            this.lassoButton.Name = "lassoButton";
            this.lassoButton.Size = new System.Drawing.Size(75, 23);
            this.lassoButton.TabIndex = 4;
            this.lassoButton.Text = "Lasso";
            this.lassoButton.UseVisualStyleBackColor = true;
            this.lassoButton.Click += new System.EventHandler(this.lassoButton_Click);
            // 
            // loop
            // 
            this.loop.Enabled = true;
            this.loop.Interval = 50;
            this.loop.Tick += new System.EventHandler(this.loop_Tick);
            // 
            // raiseEnergyLevelButtons
            // 
            this.raiseEnergyLevelButtons.Location = new System.Drawing.Point(1253, 94);
            this.raiseEnergyLevelButtons.Name = "raiseEnergyLevelButtons";
            this.raiseEnergyLevelButtons.Size = new System.Drawing.Size(75, 23);
            this.raiseEnergyLevelButtons.TabIndex = 5;
            this.raiseEnergyLevelButtons.Text = "Raise";
            this.raiseEnergyLevelButtons.UseVisualStyleBackColor = true;
            this.raiseEnergyLevelButtons.Click += new System.EventHandler(this.raiseEnergyLevelButtons_Click);
            // 
            // lowerEnergyLevelsButton
            // 
            this.lowerEnergyLevelsButton.Location = new System.Drawing.Point(1253, 123);
            this.lowerEnergyLevelsButton.Name = "lowerEnergyLevelsButton";
            this.lowerEnergyLevelsButton.Size = new System.Drawing.Size(75, 23);
            this.lowerEnergyLevelsButton.TabIndex = 6;
            this.lowerEnergyLevelsButton.Text = "Lower";
            this.lowerEnergyLevelsButton.UseVisualStyleBackColor = true;
            this.lowerEnergyLevelsButton.Click += new System.EventHandler(this.lowerEnergyLevelsButton_Click);
            // 
            // LassoDemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1340, 550);
            this.Controls.Add(this.lowerEnergyLevelsButton);
            this.Controls.Add(this.raiseEnergyLevelButtons);
            this.Controls.Add(this.lassoButton);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.mainbox);
            this.Name = "LassoDemo";
            this.Text = "LassoDemo";
            this.Load += new System.EventHandler(this.LassoDemo_Load);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.LassoDemo_MouseClick);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LassoDemo_MouseMove);
            ((System.ComponentModel.ISupportInitialize)(this.mainbox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox mainbox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button lassoButton;
        private System.Windows.Forms.Timer loop;
        private System.Windows.Forms.Button raiseEnergyLevelButtons;
        private System.Windows.Forms.Button lowerEnergyLevelsButton;
    }
}