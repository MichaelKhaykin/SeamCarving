using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeamCarving.HelperFunctions
{
    public unsafe class FastBitmap : IDisposable
    {
        private Bitmap _bmp;

        public Bitmap Bitmap
        {
            get
            {
                if (disposeCount >= 1) throw new Exception("Bitmap has already been disposed of");
                Dispose();
                return _bmp;
            }
        }

        private ImageLockMode _lockmode;
        private int _pixelLength;

        private Rectangle _rect;
        private BitmapData _data;
        private byte* _bufferPtr;

        public int Width { get => _bmp.Width; }
        public int Height { get => _bmp.Height; }
        public PixelFormat PixelFormat { get => _bmp.PixelFormat; }

        private int disposeCount = 0;
        public FastBitmap(Bitmap bmp, ImageLockMode lockMode)
        {
            _bmp = bmp;
            _lockmode = lockMode;

            _pixelLength = Image.GetPixelFormatSize(bmp.PixelFormat) / 8;
            _rect = new Rectangle(0, 0, Width, Height);
            _data = bmp.LockBits(_rect, lockMode, PixelFormat);
            _bufferPtr = (byte*)_data.Scan0.ToPointer();
        }

        public Test this[int x, int y]
        {
            get
            {
                var pixel = _bufferPtr + y * _data.Stride + x * _pixelLength;
                return (Test)pixel;
            }
            set
            {
                var pixel = _bufferPtr + y * _data.Stride + x * _pixelLength;
                var bytes = (byte*)value;
                
                for(int i = 0; i < _pixelLength; i++)
                {
                    pixel[i] = bytes[i];
                }
            }
        }

        public void Dispose()
        {
            disposeCount++;
            _bmp.UnlockBits(_data);
        }
    }
}
