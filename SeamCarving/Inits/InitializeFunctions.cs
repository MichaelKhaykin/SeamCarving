using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeamCarving.Inits
{
    public static class InitializeFunctions
    {
        public static double[,] ConvertToGrayScale(Image img, double RConversion, double GConversion, double BConversion)
        {
            double G(Color color)
            {
                return (color.R + color.G + color.B) / 3;
                //return RConversion * color.R + GConversion * color.G + BConversion * color.B;
            }

            Bitmap map = new Bitmap(img);

            var width = map.Width;
            var height = map.Height;

            double[,] grayScale = new double[height, width];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    grayScale[y, x] = G(map.GetPixel(x, y));
                }
            }

            return grayScale;
        }
    }
}
