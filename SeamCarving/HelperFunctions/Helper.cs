using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SeamCarving.HelperFunctions
{
    public static unsafe partial class Helper
    {
        public static byte GetBitsPerPixel(PixelFormat Pixelformat)
        {
            switch (Pixelformat)
            {
                case PixelFormat.Format8bppIndexed:
                    return 8;

                case PixelFormat.Format24bppRgb:
                    return 24;

                case PixelFormat.Format32bppArgb:
                case PixelFormat.Format32bppPArgb:
                    return 32;
            }
            return 0;
        }
        public static double[,] Normalize(double[,] arr)
        {
            var max = double.NegativeInfinity;
            double[,] newarr = new double[arr.GetLength(0), arr.GetLength(1)];
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    if (arr[i, j] == int.MaxValue) continue;
                    max = Math.Max(max, arr[i, j]);
                }
            }

            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    newarr[i, j] = arr[i, j] / max * 255;
                }
            }

            return newarr;
        }
        public static List<PointF> ContainedInPolygon(List<Line> polygon)
        {
            (double minX, double minY, double maxX, double maxY) = GetExtreme(polygon);

            List<PointF> p = new List<PointF>();
            for (int i = (int)minX; i <= (int)maxX; i++)
            {
                for (int j = (int)minY; j <= (int)maxY; j++)
                {
                    var ray = new Ray(new PointF(i, j), 90);

                    int count = 0;
                    foreach (var line in polygon)
                    {
                        var x = ray.Cast(line);
                        count += x == null ? 0 : 1;
                    }

                    if (count % 2 != 0)
                    {
                        p.Add(new PointF(i, j));
                    }
                }
            }

            return p;
        }
        public static (double minX, double minY, double maxX, double maxY) GetExtreme(List<Line> polygon)
        {
            double minX = double.MaxValue;
            double minY = double.MaxValue;
            double maxX = double.MinValue;
            double maxY = double.MinValue;

            foreach (var item in polygon)
            {
                minX = Math.Min(Math.Min(item.Start.X, item.End.X), minX);
                minY = Math.Min(Math.Min(item.Start.Y, item.End.Y), minY);
                maxX = Math.Max(Math.Max(item.Start.X, item.End.X), maxX);
                maxY = Math.Max(Math.Max(item.Start.Y, item.End.Y), maxY);
            }

            return (minX, minY, maxX, maxY);
        }

        public static Bitmap PlotDFT(Complex[,] data)
        {
            Bitmap map = new Bitmap(data.GetLength(1), data.GetLength(0));
            
            double largestMag = double.NegativeInfinity;
            for(int i = 0; i < data.GetLength(0); i++)
            {
                for(int j = 0; j < data.GetLength(1); j++)
                {
                    largestMag = Math.Max(data[i, j].Magnitude, largestMag);
                }
            }

            var c = 255 / Math.Log(1 + largestMag);

            for(int i = 0; i < data.GetLength(0); i++)
            {
                for(int j = 0; j < data.GetLength(1); j++)
                {
                    var currentCellData = data[i, j];
                    var val = currentCellData.Magnitude;

                    var scaled = (int)(c * Math.Log(1 + val));

                    map.SetPixel(j, i, Color.FromArgb(255, scaled, scaled, scaled));
                }
            }

            return map;
        }
    }
}
