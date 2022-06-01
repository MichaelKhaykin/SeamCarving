using SeamCarving.HelperFunctions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SeamCarving.HelperFunctions.Helper;

namespace SeamCarving.Drawing
{
    public unsafe static class DrawExtensions
    {
        public static Bitmap EnergyLevelsToBitMapSetPixelVersion(double[,] energyLevels)
        {
            var height = energyLevels.GetLength(0);
            var width = energyLevels.GetLength(1);

            Bitmap map = new Bitmap(width, height);
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    var lev = (byte)Math.Min(255, energyLevels[i, j]);
                    map.SetPixel(j, i, Color.FromArgb(255, lev, lev, lev));
                }
            }
            return map;
        }
        public static Bitmap EnergyLevelsToBitMapLockBitsVersion(double[,] energyLevels)
        {
            var height = energyLevels.GetLength(0);
            var width = energyLevels.GetLength(1);

            Bitmap map = new Bitmap(width, height);
            FastBitmap fast = new FastBitmap(map, ImageLockMode.ReadWrite);

            Dictionary<int, Test> maybe = new Dictionary<int, Test>();

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    var lev = (byte)Math.Min(255, energyLevels[i, j]);

                    if (!maybe.ContainsKey(lev))
                    {
                        maybe.Add(lev, new Test(Color.FromArgb(255, lev, lev, lev)));
                    }
                    fast[j, i] = maybe[lev];
                }

            }

            return fast.Bitmap;
        }
        public static void DrawSeam(PictureBox box, HashSet<(int, int)> seamPath)
        {
            box.Image = DrawSeam(new Bitmap(box.Image), seamPath);
        }
        public static Bitmap DrawSeam(Bitmap map, HashSet<(int, int)> seamPath)
        {
            foreach (var point in seamPath)
            {
                map.SetPixel(point.Item2, point.Item1, Color.Red);
            }
            return map;
        }
        public static Bitmap ChangeOgImageX(Bitmap og, HashSet<(int, int)> path)
        {
            int height = og.Height;
            int width = og.Width;

            Bitmap newMap = new Bitmap(width - 1, height);

            for (int i = 0; i < height; i++)
            {
                int xAxis = 0;
                for (int j = 0; j < width; j++)
                {
                    if (path.Contains((i, j))) continue;

                    newMap.SetPixel(xAxis, i, og.GetPixel(j, i));
                    xAxis++;
                }
            }
            return newMap;
        }

        public static Bitmap ChangeOgImageY(Bitmap og, HashSet<(int, int)> path)
        {
            int height = og.Height;
            int width = og.Width;

            Bitmap newMap = new Bitmap(width, height - 1);

            var skipIndex = path.First().Item1;
            int yAxis = 0;

            for (int i = 0; i < height; i++)
            {
                if (i == skipIndex) continue;

                for (int j = 0; j < width; j++)
                {
                    newMap.SetPixel(j, yAxis, og.GetPixel(j, i));
                }
                yAxis++;
            }
            return newMap;
        }


        public static Bitmap DpVisualizer(double[,] dp)
        {
            var norm = Helper.Normalize(dp);

            var img = EnergyLevelsToBitMapLockBitsVersion(norm);
            return img;
        }
        public static Bitmap BlowUp(Image original, int scale)
        {
            Bitmap map = new Bitmap(original);

            int blownUpX = 0;
            int blownUpY = 0;

            var blownUpSize = new Size(original.Width * scale, original.Height * scale);
            Bitmap blownUpMap = new Bitmap(blownUpSize.Width, blownUpSize.Height);
         
            FastBitmap faster = new FastBitmap(blownUpMap, ImageLockMode.ReadWrite);

            for (int i = 0; i < original.Width; i++)
            {
                for (int j = 0; j < original.Height; j++)
                {
                    var color = map.GetPixel(i, j);

                    for (int x = 0; x < scale; x++)
                    {
                        for (int y = 0; y < scale; y++)
                        {
                            faster[blownUpX + x, blownUpY + y] = (Test)color;
                        }
                    }

                    blownUpY += scale;
                }
                blownUpX += scale;
                blownUpY = 0;
            }

            return faster.Bitmap;
        }
        public static Bitmap BlowUp(Bitmap original, int scale)
        {
            Bitmap map = new Bitmap(original);

            int blownUpX = 0;
            int blownUpY = 0;

            var blownUpSize = new Size(original.Width * scale, original.Height * scale);
            Bitmap blownUpMap = new Bitmap(blownUpSize.Width, blownUpSize.Height);

            FastBitmap faster = new FastBitmap(blownUpMap, ImageLockMode.ReadWrite);

            for (int i = 0; i < original.Width; i++)
            {
                for (int j = 0; j < original.Height; j++)
                {
                    var color = map.GetPixel(i, j);

                    for (int x = 0; x < scale; x++)
                    {
                        for (int y = 0; y < scale; y++)
                        {
                            faster[blownUpX + x, blownUpY + y] = (Test)color;
                        }
                    }

                    blownUpY += scale;
                }
                blownUpX += scale;
                blownUpY = 0;
            }

            return faster.Bitmap;
        }
        public static Bitmap GaussianBlur(Bitmap original)
        {
            int[,] matrix = new int[,]
            {
                { 1, 1, 1, 1, 1 },
                { 1, 1, 1, 1, 1 },
                { 1, 1, 1, 1, 1 },
                { 1, 1, 1, 1, 1 },
                { 1, 1, 1, 1, 1 },
            };

            int offset = matrix.GetLength(0) / 2;

            int matrixSum = 0;
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    matrixSum += matrix[i, j];
                }
            }

            Bitmap map = new Bitmap(original);

            for (int i = offset; i < original.Height - offset; i++)
            {
                for (int j = offset; j < original.Width - offset; j++)
                {
                    int blurredValR = 0;
                    int blurredValG = 0;
                    int blurredValB = 0;
                    for (int y = 0; y < matrix.GetLength(0); y++)
                    {
                        for (int x = 0; x < matrix.GetLength(1); x++)
                        {
                            var color = original.GetPixel(x + j - offset, y + i - offset);
                            var r = color.R;
                            var g = color.G;
                            var b = color.B;

                            blurredValR += r * matrix[y, x];
                            blurredValG += g * matrix[y, x];
                            blurredValB += b * matrix[y, x];
                        }
                    }
                    blurredValR /= matrixSum;
                    blurredValG /= matrixSum;
                    blurredValB /= matrixSum;

                    map.SetPixel(j, i, Color.FromArgb(blurredValR, blurredValG, blurredValB));
                }
            }

            return map;
        }
        public static Image RotateImage(Image img, float rotationAngle)
        {
            //create an empty Bitmap image
            Bitmap bmp = new Bitmap(img.Width, img.Height);

            //turn the Bitmap into a Graphics object
            Graphics gfx = Graphics.FromImage(bmp);

            //now we set the rotation point to the center of our image
            gfx.TranslateTransform((float)bmp.Width / 2, (float)bmp.Height / 2);

            //now rotate the image
            gfx.RotateTransform(rotationAngle);

            gfx.TranslateTransform(-(float)bmp.Width / 2, -(float)bmp.Height / 2);

            //set the InterpolationMode to HighQualityBicubic so to ensure a high
            //quality image once it is transformed to the specified size
            gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;

            //now draw our new image onto the graphics object
            gfx.DrawImage(img, new Point(0, 0));

            //dispose of our Graphics object
            gfx.Dispose();

            //return the image
            return bmp;
        }
    }
}
