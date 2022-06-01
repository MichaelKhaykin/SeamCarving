using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeamCarving.HelperFunctions
{
    public class Line
    {
        public PointF Start { get; }
        public PointF End { get; }
        public Line(PointF s, PointF e)
        {
            Start = s;
            End = e;
        }
    }
}
