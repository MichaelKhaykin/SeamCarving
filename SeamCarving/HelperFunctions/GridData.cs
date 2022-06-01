using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeamCarving.HelperFunctions
{
    public class GridData
    {
        public int OffSetX { get; set; }
        public int OffSetY { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int SpacingX { get; set; }
        public int SpacingY { get; set; }
    
        public int[,] Matrix { get; }
        public GridData(int offsetX, int offsetY, int spacing, int[,] matrix)
        {
            OffSetX = offsetX;
            OffSetY = offsetY;

            Width = CalculateWidth(matrix.GetLength(1), spacing);
            Height = CalculateHeight(matrix.GetLength(0), spacing);

            SpacingX = spacing;
            SpacingY = spacing;

            Matrix = matrix;
        }

        private int CalculateWidth(int matrixWidth, int spacing)
        {
            return matrixWidth * spacing;
        }
        private int CalculateHeight(int matrixHeight, int spacing)
        {
            return matrixHeight * spacing;
        }

        public bool Contains(Point p)
        {
            if (p.X < OffSetX) return false;
            if (p.X > OffSetX + Width) return false;
            if (p.Y < OffSetY) return false;
            if (p.Y > OffSetY + Height) return false;

            return true;
        }
    }
}
