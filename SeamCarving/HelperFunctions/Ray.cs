using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeamCarving.HelperFunctions
{
    public class Ray
    {
        public PointF Position { get; set; }
        public float Angle { get; set; }
        public PointF Direction
        {
            get
            {
                return new PointF((float)Math.Cos(Angle * Math.PI / 180), (float)Math.Sin(Angle * Math.PI / 180));
            }
        }

        public Ray(PointF position, float angle)
        {
            Angle = angle;
            Position = position;
        }

        public PointF? Cast(Line line)
        {
            var x1 = line.Start.X;
            var x2 = line.End.X;
            var x3 = Position.X;
            var x4 = Position.X + Direction.X;

            var y1 = line.Start.Y;
            var y2 = line.End.Y;
            var y3 = Position.Y;
            var y4 = Position.Y + Direction.Y;

            double denominator = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);

            if (denominator == 0) return null;

            var t = ((x1 - x3) * (y3 - y4) - (y1 - y3) * (x3 - x4)) / denominator;
            var u = -((x1 - x2) * (y1 - y3) - (y1 - y2) * (x1 - x3)) / denominator;

            double playwith = 0;
            double second = 1;
            if (t <= playwith || t >= second || u <= playwith) return null;

            double x = x1 + t * (x2 - x1);
            double y = y1 + t * (y2 - y1);
            PointF point = new PointF((float)x, (float)y);
            return point;
        }
    }
}
