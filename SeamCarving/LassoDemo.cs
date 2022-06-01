using SeamCarving.CoreLogic;
using SeamCarving.Drawing;
using SeamCarving.HelperFunctions;
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
    public partial class LassoDemo : Form
    {
        MouseButtons prev = MouseButtons.None;
        MouseButtons curr;

        int points = 0;
        Pen red;
        int r = 10;
        Point temp;

        Point originalStart;
        List<Line> lines = new List<Line>();

        Graphics g;

        PictureBox generated;
        Image persistent;
        PictureBox news;
        PictureBox dpVisualizer;

        bool lassoActive = false;

        double[,] energyLevels;

        Bitmap changing;
        public LassoDemo()
        {
            InitializeComponent();

            temp = new Point(-1, -1);
            originalStart = temp;
            WindowState = FormWindowState.Maximized;

            red = new Pen(Brushes.Red, 2);
        }

        private void LassoDemo_Load(object sender, EventArgs e)
        {
            g = CreateGraphics();

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
            news.MouseMove += LassoDemo_MouseMove;
            news.MouseClick += LassoDemo_MouseClick;

            dpVisualizer = new PictureBox()
            {
                Location = new Point(mainbox.Right + 15, mainbox.Location.Y),
                Size = mainbox.Size,
            };

            dpVisualizer.Image = DrawExtensions.DpVisualizer(MainComponent.GenerateDP(energyLevels));

            changing = new Bitmap(mainbox.Image, generated.Image.Size);

            persistent = mainbox.Image;

            Controls.Add(news);
            Controls.Add(generated);
            Controls.Add(dpVisualizer);

            button1.Click += button1_Click;
            button2.Click += button2_Click;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var dp = MainComponent.GenerateDP(energyLevels);
            dpVisualizer.Image = DrawExtensions.DpVisualizer(dp);

            var seam = MainComponent.FindSeam(dp);
            changing = DrawExtensions.DrawSeam(changing, seam);

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
            changing = DrawExtensions.DrawSeam(changing, seam);
          
            news.Image = changing;
        }

        private void lassoButton_Click(object sender, EventArgs e)
        {
            persistent = news.Image;
            lassoActive = !lassoActive;
        }

        private void LassoDemo_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void LassoDemo_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void loop_Tick(object sender, EventArgs e)
        {
            curr = Control.MouseButtons;

            /* Debug
            if(lassoActive == false)
            {
                Bitmap m = new Bitmap(news.Image);
                Graphics g = Graphics.FromImage(m);
                foreach(var line in lines)
                {
                    g.DrawLine(Pens.Blue, line.Start, line.End);
                }
                news.Image = m;
                return;
            }
            */

            var pos = PointToClient(Cursor.Position);
            var local = new Point(pos.X - news.Location.X, pos.Y - news.Location.Y);
            var center = new Point(local.X - r / 2, local.Y - r / 2);

            g.Clear(this.BackColor);

            if (news.Bounds.Contains(pos) && lassoActive)
            {
                
                if (curr == MouseButtons.Left && prev == MouseButtons.None)
                {
                    points++;
                    persistent = news.Image;

                    var barb = Graphics.FromImage(persistent);

                    if (originalStart.X != -1 && originalStart.Y != -1 && points > 2)
                    {
                        var normalizedPoint = new Point(Math.Abs(local.X - originalStart.X), Math.Abs(local.Y - originalStart.Y));
                        if (normalizedPoint.X * normalizedPoint.X + normalizedPoint.Y * normalizedPoint.Y <= r * r)
                        {
                            barb.DrawEllipse(new Pen(Brushes.Red, 2), originalStart.X - r / 2, originalStart.Y - r / 2, r, r);
                            lines.Add(new Line(temp, originalStart));

                            // l.DrawEllipse(red, originalStart.X - r / 2, originalStart.Y - r / 2, r, r);
                            //barb.DrawLine(Pens.Red, temp, originalStart);

                            news.Image = persistent;
                            lassoActive = false;
                            return;
                        }
                    }

                    if (temp.X == -1 && temp.Y == -1)
                    {
                        originalStart = local;
                    }

                    if (temp.X != -1 && temp.Y != -1)
                    {
                        lines.Add(new Line(temp, local));
                    }

                    temp = local;

                    barb.DrawEllipse(red, center.X, center.Y, r, r);
                }

                news.Image = persistent;

                Bitmap map = new Bitmap(news.Image);
                var l = Graphics.FromImage(map);
                
                l.DrawEllipse(red, new RectangleF(center.X, center.Y, r, r));

                if (originalStart.X != -1 && originalStart.Y != -1 && points > 2)
                {
                    var normalizedPoint = new Point(Math.Abs(local.X - originalStart.X), Math.Abs(local.Y - originalStart.Y));
                    if (normalizedPoint.X * normalizedPoint.X + normalizedPoint.Y * normalizedPoint.Y <= r * r)
                    {
                        l.DrawEllipse(new Pen(Brushes.Yellow, 2), originalStart.X - r / 2, originalStart.Y - r / 2, r, r);
                    }
                }

                
                if (temp.X != -1 && temp.Y != -1)
                {
                    l.DrawLine(Pens.Red, temp, local);
                }
                else
                {
                    l.DrawEllipse(red, new RectangleF(local.X - r / 2, local.Y - r / 2, r, r));
                }

                news.Image = map;
            }
            else if (lassoActive)
            {
                news.Image = persistent;
                g.DrawEllipse(red, new RectangleF(pos.X - r / 2, pos.Y - r / 2, r, r));
            }

            prev = curr;
        }

        private void raiseEnergyLevelButtons_Click(object sender, EventArgs e)
        {
            var allPoints = Helper.ContainedInPolygon(lines);

            double max = double.NegativeInfinity;
            foreach(var item in energyLevels)
            {
               max = Math.Max(item, max);
            }

            int width = energyLevels.GetLength(1);
            int height = energyLevels.GetLength(0);

            double belly = 0;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (energyLevels[j, i] != int.MaxValue)
                    {
                        belly += energyLevels[j, i];
                    }
                }
            }
            belly /= (width * height);

            foreach (var item in allPoints)
            {
                energyLevels[(int)item.Y, (int)item.X] = belly;
            }
            generated.Image = DrawExtensions.EnergyLevelsToBitMapLockBitsVersion(energyLevels);
        }

        private void lowerEnergyLevelsButton_Click(object sender, EventArgs e)
        {

        }
    }
}
