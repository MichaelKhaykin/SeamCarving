using SeamCarving.CoreLogic;
using SeamCarving.Drawing;
using SeamCarving.Inits;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeamCarving
{
    public partial class VectorDemo : Form
    {
        PictureBox generated;
        PictureBox news;
        PictureBox dpVisualizer;
        PictureBox blownUpImage;

        double[,] energyLevels;
        double[,] delXVectors;
        double[,] delYVectors;

        Bitmap changing;
        Bitmap x;

        Graphics gfx;

        double max;

        double bitch = 10;

        int scale = 5;

        Bitmap blownUpMap;
        public VectorDemo()
        {
            InitializeComponent();

            WindowState = FormWindowState.Maximized;
        }

        private void VectorDemo_Load(object sender, EventArgs e)
        {
            //mainbox.Image = Properties.Resources.smallMario;


            /*
            var blurred = GaussianBlur(new Bitmap(pictureBox1.Image));
            pictureBox4.Image = BlowUp(mainbox.Image, scale);
            pictureBox3.Image = blurred;
            pictureBox2.Image = BlowUp(blurred, scale);
            */


            blownUpMap = DrawExtensions.BlowUp(mainbox.Image, scale);
            var blownUpSize = blownUpMap.Size;

            var grayScale = InitializeFunctions.ConvertToGrayScale(mainbox.Image, Constants.RConversion, Constants.GConversion, Constants.BConversion);

            //var dft = MainComponent.DFT(grayScale);
            //pictureBox1.Image = HelperFunctions.Helper.PlotDFT(dft);

            //pictureBox2.Image = DrawExtensions.BlowUp(mainbox.Image, scale);
            //pictureBox3.Image = DrawExtensions.BlowUp(pictureBox1.Image, scale);


            var blur = MainComponent.GaussianBlur(grayScale);

            var dels = MainComponent.GetDels(grayScale);
            delXVectors = dels[0];
            delYVectors = dels[1];

            energyLevels = MainComponent.ComputeEnergyLevels(delXVectors, delYVectors);
            var energyLevelMap = DrawExtensions.EnergyLevelsToBitMapLockBitsVersion(energyLevels);

            generated = new PictureBox()
            {
                Location = new Point(mainbox.Location.X, mainbox.Bottom + 15),
                Size = mainbox.Image.Size,
                Image = energyLevelMap
            };

            news = new PictureBox()
            {
                Location = new Point(mainbox.Right + 15, mainbox.Bottom + 15),
                Size = mainbox.Size,
                Image = mainbox.Image
            };

            dpVisualizer = new PictureBox()
            {
                Location = new Point(mainbox.Right + 15, mainbox.Location.Y),
                Size = mainbox.Size,
            };

            blownUpImage = new PictureBox()
            {
                Location = new Point(ClientSize.Width / 2 - blownUpSize.Width / 2, ClientSize.Height / 2 - blownUpSize.Height / 2),
                Image = blownUpMap,
                Size = blownUpSize,
            };

            var showXCheckBox = new CheckBox()
            {
                Location = new Point(ClientSize.Width - 100, 100),
                Text = "Show X Vectors",
                AutoSize = true,
            };
            showXCheckBox.CheckedChanged += ShowXCheckBox_CheckedChanged;

            var showYCheckBox = new CheckBox()
            {
                Location = new Point(showXCheckBox.Location.X, showXCheckBox.Bottom),
                Text = "Show Y Vectors",
                AutoSize = true
            };
            showYCheckBox.CheckedChanged += ShowYCheckBox_CheckedChanged;

            var energyCheckBox = new CheckBox()
            {
                Location = new Point(showXCheckBox.Location.X, showYCheckBox.Bottom),
                Text = "Show Energy",
                AutoSize = true,
            };
            energyCheckBox.CheckedChanged += EnergyCheckBox_CheckedChanged;

            dpVisualizer.Image = DrawExtensions.DpVisualizer(MainComponent.GenerateDP(energyLevels));

            changing = new Bitmap(mainbox.Image, generated.Image.Size);

            Controls.Add(news);
            Controls.Add(generated);
            Controls.Add(dpVisualizer);
            Controls.Add(blownUpImage);
            Controls.Add(showXCheckBox);
            Controls.Add(showYCheckBox);
            Controls.Add(energyCheckBox);

            x = new Bitmap(blownUpImage.Image);
            gfx = Graphics.FromImage(x);

            max = int.MinValue;
            for (int i = 0; i < delXVectors.GetLength(0); i++)
            {
                for (int j = 0; j < delXVectors.GetLength(1); j++)
                {
                    max = Math.Max(max, delXVectors[i, j]);
                }
            }
        }
        
        private void EnergyCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked == false)
            {
                gfx.Clear(this.BackColor);
                gfx.DrawImage(blownUpMap, new Point(0, 0));
                blownUpImage.Image = x;
                return;
            }

            int blownUpX = 0;
            int blownUpY = 0;
            for (int i = 0; i < energyLevels.GetLength(0); i++)
            {
                for (int j = 0; j < energyLevels.GetLength(1); j++)
                {
                    if (Math.Abs(energyLevels[i, j]) < bitch)
                    {
                        blownUpX += scale;
                        continue;
                    }

                    var initialPoint = new Point(blownUpX + scale / 2, blownUpY + scale / 2);
                    Image original = Properties.Resources.linetest;

                    var rotationAmount = (float)Math.Atan(delYVectors[i, j] / delXVectors[i, j]) * (float)(180 / Math.PI);
                    var rotatedImage = DrawExtensions.RotateImage(original, rotationAmount);
                    gfx.DrawImage(rotatedImage, initialPoint);

                    blownUpX += scale;
                }
                blownUpY += scale;
                blownUpX = 0;
            }

            blownUpImage.Image = x;
        }
        private void ShowYCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked == false)
            {
                gfx.Clear(this.BackColor);
                gfx.DrawImage(blownUpMap, new Point(0, 0));
                blownUpImage.Image = x;
                return;
            }

            Image vertical = DrawExtensions.RotateImage(Properties.Resources.linetest, 90);
            Image flippedVertical = DrawExtensions.RotateImage(Properties.Resources.linetest, 270);

            int blownUpX = 0;
            int blownUpY = 0;
            for (int i = 0; i < delYVectors.GetLength(0); i++)
            {
                for (int j = 0; j < delYVectors.GetLength(1); j++)
                {
                    var normalized = -(delYVectors[i, j] / max) * 10;

                    if (Math.Abs(delYVectors[i, j]) < bitch)
                    {
                        blownUpX += scale;
                        continue;
                    }

                    var initialPoint = new Point(blownUpX + scale / 2, blownUpY + scale / 2);
                    gfx.DrawImage(normalized > 0 ? vertical : flippedVertical, initialPoint);

                    blownUpX += scale;
                }
                blownUpY += scale;
                blownUpX = 0;
            }

            blownUpImage.Image = x;
        }
        private void ShowXCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked == false)
            {
                gfx.Clear(this.BackColor);
                gfx.DrawImage(blownUpMap, new Point(0, 0));
                blownUpImage.Image = x;
                return;
            }

            Image nonRotated = Properties.Resources.linetest;
            Image flipped = DrawExtensions.RotateImage(Properties.Resources.linetest, 180);

            int blownUpX = 0;
            int blownUpY = 0;
            for (int i = 0; i < delXVectors.GetLength(0); i++)
            {
                for (int j = 0; j < delXVectors.GetLength(1); j++)
                {
                    var normalized = -(delXVectors[i, j] / max) * 10;

                    if (Math.Abs(delXVectors[i, j]) < bitch)
                    {
                        blownUpX += scale;
                        continue;
                    }

                    var initialPoint = new Point(blownUpX + scale / 2, blownUpY + scale / 2);
                    gfx.DrawImage(normalized > 0 ? nonRotated : flipped, initialPoint);

                    blownUpX += scale;
                }
                blownUpY += scale;
                blownUpX = 0;
            }

            blownUpImage.Image = x;
        }
    }
}
