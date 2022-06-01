using SeamCarving.CoreLogic;
using SeamCarving.Drawing;
using SeamCarving.Inits;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeamCarving
{
    public partial class ClockDemo : Form
    {
        PictureBox generated;
        PictureBox news;
        PictureBox dpVisualizer;

        double[,] energyLevels;

        Bitmap changing;
        public ClockDemo()
        {
            InitializeComponent();
            
            WindowState = FormWindowState.Maximized;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            mainbox.Image = Properties.Resources.timelessclock;

            var grayScale = InitializeFunctions.ConvertToGrayScale(mainbox.Image, Constants.RConversion, Constants.GConversion, Constants.BConversion);
            
            var dels = MainComponent.GetDels(grayScale);
            var delX = dels[0];
            var delY = dels[1];

            energyLevels = MainComponent.ComputeEnergyLevels(delX, delY);

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

            dpVisualizer.Image = DrawExtensions.DpVisualizer(MainComponent.GenerateDP(energyLevels));

            changing = new Bitmap(mainbox.Image, generated.Image.Size);

            Controls.Add(news);
            Controls.Add(generated);
            Controls.Add(dpVisualizer);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var dp = MainComponent.GenerateDP(energyLevels);
            dpVisualizer.Image = DrawExtensions.DpVisualizer(dp);

            var seam = MainComponent.FindSeam(dp);
            DrawExtensions.DrawSeam(changing, seam);

            energyLevels = MainComponent.CarveXAxis(energyLevels, seam);
            generated.Image = DrawExtensions.EnergyLevelsToBitMapLockBitsVersion(energyLevels);

            changing = DrawExtensions.ChangeOgImageX(changing, seam);

            news.Image = changing;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            var dp = MainComponent.GenerateDP(energyLevels);
            dpVisualizer.Image = DrawExtensions.DpVisualizer(dp);

            var seam = MainComponent.FindSeam(dp);
            Stopwatch s = new Stopwatch();
            s.Start();
            DrawExtensions.DrawSeam(changing, seam);
            s.Stop();

            news.Image = changing;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void HorizontalCut_Click(object sender, EventArgs e)
        {
            var dp = MainComponent.GenerateDPHorizontally(energyLevels);
            var seam = MainComponent.FindHorizontalSeam(dp);

            DrawExtensions.DrawSeam(changing, seam);

            energyLevels = MainComponent.CarveYAxis(energyLevels, seam);
            generated.Image = DrawExtensions.EnergyLevelsToBitMapLockBitsVersion(energyLevels);

            changing = DrawExtensions.ChangeOgImageY(changing, seam);

            news.Image = changing;
        }

        private void CutHorizontalSeam_Click(object sender, EventArgs e)
        {
            var dp = MainComponent.GenerateDPHorizontally(energyLevels);
           
            var seam = MainComponent.FindHorizontalSeam(dp);
            Stopwatch s = new Stopwatch();
            s.Start();
            DrawExtensions.DrawSeam(changing, seam);
            s.Stop();

            news.Image = changing;
        }
    }
}
