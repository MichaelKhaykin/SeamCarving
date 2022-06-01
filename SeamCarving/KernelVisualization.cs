using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace SeamCarving
{
    public partial class KernelVisualization : Form
    {
        Graphics gfx;
        Bitmap draw;

        HelperFunctions.GridData KernelData;
        HelperFunctions.GridData ImageData;

        List<Rectangle> flashed = new List<Rectangle>();
        List<HelperFunctions.Line> test = new List<HelperFunctions.Line>();
        Stopwatch showAmount = new Stopwatch();
        long elapsed = 500;
        int index = 0;
        int runningSum = 0;

        List<string> current = new List<string>();
        StringBuilder sofar = new StringBuilder();
        public KernelVisualization()
        {

            WindowState = FormWindowState.Maximized;
            InitializeComponent();

        }

       
        private void KernelVisualization_Load(object sender, EventArgs e)
        {
            mainBox.Location = new Point(0, 0);
            mainBox.Width = ClientSize.Width;
            mainBox.Height = ClientSize.Height;

            draw = new Bitmap(mainBox.Width, mainBox.Height);
            gfx = Graphics.FromImage(draw);

            int spacing = 50;


            int[,] kernel = new int[,]
            {
                { -1, 0, 1 },
                { -2, 0, 2 },
                { -1, 0, 1 }
            };

            int[,] image = new int[,]
            {
                { 50, 50, 100, 100, 0, 50 },
                { 50, 50, 100, 100, 0, 50 },
                { 50, 50, 100, 100, 0, 50 },
                { 50, 50, 100, 100, 0, 50 },
                { 50, 50, 100, 100, 0, 50 },
                { 50, 50, 100, 100, 0, 50 },
            };


            KernelData = new HelperFunctions.GridData(400, 400, spacing, kernel);
            ImageData = new HelperFunctions.GridData(50, 50, spacing, image);

            label1.Location = new Point(ImageData.OffSetX + ImageData.Width + 50, ImageData.OffSetY + ImageData.Height / 2);
        }

        private void DrawGrid(HelperFunctions.GridData data)
        {
            for (int i = data.OffSetY; i <= data.OffSetY + data.Height; i += data.SpacingY)
            {
                gfx.DrawLine(new Pen(Brushes.Red, 1), new Point(data.OffSetX, i), new Point(data.OffSetX + data.Width, i));
            }

            for (int i = data.OffSetX; i <= data.OffSetX + data.Width; i += data.SpacingX)
            {
                gfx.DrawLine(new Pen(Brushes.Red, 1), new Point(i, data.OffSetY), new Point(i, data.OffSetY + data.Height));
            }
        }

        private Point IndexToPosition(HelperFunctions.GridData l, (int x, int y) pos)
        {
            return new Point(l.OffSetX + l.SpacingX * pos.x, l.OffSetY + l.SpacingY * pos.y);
        }
        private List<Rectangle> Apply(HelperFunctions.GridData image, HelperFunctions.GridData kernal, (int x, int y) position)
        {
            var kernel = kernal.Matrix;

            if (position.x - 1 < 0 || position.x + 1 > image.Matrix.GetLength(1) || position.y - 1 < 0 || position.y + 1 > image.Matrix.GetLength(0)) return null;

            test = new List<HelperFunctions.Line>();

            List<Rectangle> rectanglesToFlash = new List<Rectangle>();

            for (int i = 0; i < kernel.GetLength(0); i++)
            {
                for (int j = 0; j < kernel.GetLength(1); j++)
                {
                    //j + x - 1
                    //i + y - 1
                    var adjX = j + position.x - 1;
                    var adjY = i + position.y - 1;

                    Point p = IndexToPosition(image, (adjX, adjY));
                    Point o = IndexToPosition(kernal, (j, i));

                    test.Add(new HelperFunctions.Line(p, o));
                    test.Add(new HelperFunctions.Line(new PointF(p.X + image.SpacingX, p.Y), new PointF(o.X + kernal.SpacingX, o.Y)));
                    test.Add(new HelperFunctions.Line(new PointF(p.X, p.Y + image.SpacingY), new PointF(o.X, o.Y + kernal.SpacingY)));
                    test.Add(new HelperFunctions.Line(new PointF(p.X + image.SpacingX, p.Y + image.SpacingY), new PointF(o.X + kernal.SpacingX, o.Y + kernal.SpacingY)));

                    rectanglesToFlash.Add(new Rectangle(p.X + 1, p.Y + 1, image.SpacingX - 1, image.SpacingY - 1));
                    rectanglesToFlash.Add(new Rectangle(o.X + 1, o.Y + 1, kernal.SpacingX - 1, kernal.SpacingY - 1));

                    current.Add($"{kernel[i, j]} * {image.Matrix[adjY, adjX]}");
                }
            }

            return rectanglesToFlash;
        }



        private void DrawNumbers(HelperFunctions.GridData matrixData, int[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    var font = $"{matrix[i, j]}";
                    var size = gfx.MeasureString(font, this.Font);

                    var pnt = IndexToPosition(matrixData, (j, i));

                    var centered = new PointF(pnt.X + matrixData.SpacingX / 2 - size.Width / 2,
                                             pnt.Y + matrixData.SpacingY / 2 - size.Height / 2);

                    gfx.DrawString($"{matrix[i, j]}", this.Font, Brushes.Black, centered);
                }
            }
        }

        private int Calculate(string s)
        {
            var arr = s.Split('*');
            var first = int.Parse(arr[0]);
            var second = int.Parse(arr[1]);

            return first * second;
        }
        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            gfx.Clear(this.BackColor);

            DrawGrid(KernelData);
            DrawGrid(ImageData);

            var pos = PointToClient(Control.MousePosition);

            label1.Text = sofar.ToString();

            if (Control.MouseButtons == MouseButtons.Left && ImageData.Contains(Control.MousePosition))
            {
                var local = new Point(pos.X - ImageData.OffSetX, pos.Y - ImageData.OffSetY);

                var xIndex = local.X / ImageData.SpacingX;
                var yIndex = local.Y / ImageData.SpacingY;

                current.Clear();
                label1.Text = "";
                sofar.Clear();
                runningSum = 0;
                flashed = Apply(ImageData, KernelData, (xIndex, yIndex));
                if (flashed != null)
                {
                    showAmount.Start();
                }
                index = 0;
            }

            if(test != null && test.Count > 0)
            {
                for (int i = 0; i < 2; i++)
                {
                    gfx.FillRectangle(Brushes.Yellow, flashed[i + index / 2]);
                }
                for (int i = 0; i < 4; i++)
                {
                    gfx.DrawLine(Pens.Black, test[index + i].Start, test[index + i].End);
                }
            }

            if (test != null && test.Count > 0 && showAmount.ElapsedMilliseconds > elapsed)
            {
                sofar.Append(current[index / 4] + " " + "+" + " ");
                runningSum += Calculate(current[index / 4]);
                index += 4;
                if(index >= test.Count)
                {
                    sofar.Remove(sofar.Length - 2, 2);
                    sofar.Append($"= {runningSum}");
                    test = null;
                }
                showAmount.Restart();
            }

            DrawNumbers(KernelData, KernelData.Matrix);
            DrawNumbers(ImageData, ImageData.Matrix);

            mainBox.Image = draw;
        }

        private void mainBox_Click(object sender, EventArgs e)
        {

        }
    }
}
