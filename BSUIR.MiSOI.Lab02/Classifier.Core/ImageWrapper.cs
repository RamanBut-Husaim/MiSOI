using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Classifier.Core
{
    public sealed class ImageWrapper
    {
        private byte[][] _imageData;
        private int _width;
        private int _height;
        private int _stride;

        public ImageWrapper(Bitmap image)
        {
            this.Initialize(image);
        }

        public int Width
        {
            get
            {
                return this._width;
            }
        }

        public int Height
        {
            get
            {
                return this._height;
            }
        }

        public byte this[int i, int j]
        {
            get
            {
                return this._imageData[i][j];
            }

            set
            {
                this._imageData[i][j] = value;
            }
        }

        public byte[] ToByteArray()
        {
            int imageSize = Math.Abs(this._stride) * this._height;
            var result = new byte[imageSize];

            for (int i = 0; i < this._height; ++i)
            {
                for (int j = 0; j < this._width; ++j)
                {
                    result[(i * this._stride) + (j * 3)] = this._imageData[i][j];
                    result[(i * this._stride) + ((j * 3) + 1)] = this._imageData[i][j];
                    result[(i * this._stride) + ((j * 3) + 2)] = this._imageData[i][j];
                }
            }

            return result;
        }

        public int GetColoredPixels(byte color)
        {
            int result = 0;

            for (int i = 0; i < this.Height; ++i)
            {
                for (int j = 0; j < this.Width; ++j)
                {
                    if (this._imageData[i][j] == color)
                    {
                        result++;
                    }
                }
            }

            return result;
        }

        public byte[] GetHistogramm()
        {
            var histogramm = new byte[byte.MaxValue + 1];

            for (int i = 0; i < this.Height; ++i)
            {
                for (int j = 0; j < this.Width; ++j)
                {
                    histogramm[this._imageData[i][j]] += 1;
                }
            }

            return histogramm;
        }

        private void Initialize(Bitmap bitMap)
        {
            var rect = new Rectangle(0, 0, bitMap.Width, bitMap.Height);
            BitmapData bitmapData = bitMap.LockBits(rect, ImageLockMode.ReadOnly, bitMap.PixelFormat);
            IntPtr startPtr = bitmapData.Scan0;

            int imageSize = Math.Abs(bitmapData.Stride) * bitMap.Height;
            var rgbBytes = new byte[imageSize];
            Marshal.Copy(startPtr, rgbBytes, 0, imageSize);

            this._imageData = new byte[bitMap.Height][];
            this._width = bitMap.Width;
            this._height = bitMap.Height;
            this._stride = bitmapData.Stride;

            for (int i = 0; i < bitMap.Height; ++i)
            {
                this._imageData[i] = new byte[bitMap.Width];
                for (int j = 0; j < bitMap.Width; ++j)
                {
                    byte b = rgbBytes[(i * bitmapData.Stride) + (j * 3)];
                    byte g = rgbBytes[(i * bitmapData.Stride) + ((j * 3) + 1)];
                    byte r = rgbBytes[(i * bitmapData.Stride) + ((j * 3) + 2)];
                    this._imageData[i][j] = (byte)(((int)r + (int)g + (int)b) / 3);
                }
            }

            bitMap.UnlockBits(bitmapData);
        }
    }
}
