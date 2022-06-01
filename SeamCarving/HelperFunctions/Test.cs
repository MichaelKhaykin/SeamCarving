using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeamCarving.HelperFunctions
{
    public unsafe struct Test
    {
        byte* a;
        public Test(byte* a)
        {
            this.a = a;
        }

        public Test(Color col)
        {
            var byteArray = new byte[4];

            fixed (byte* c = byteArray)
            {
                c[2] = col.R;
                c[1] = col.G;
                c[0] = col.B;
                c[3] = col.A;

                a = c;
            }
        }

        public static implicit operator byte*(Test x) => x.a;
        public static explicit operator Test(byte* b) => new Test(b);
        public static explicit operator Test(Color c) => new Test(c); 
    }
}
