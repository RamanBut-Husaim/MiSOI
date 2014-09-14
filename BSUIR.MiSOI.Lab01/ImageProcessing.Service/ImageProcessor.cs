using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ImageProcessing.Service
{
    public sealed class ImageProcessor : IImageProcessor
    {
        private readonly Bitmap _bitmap;

        private bool _disposed;

        public ImageProcessor(string filePath)
        {
            this._bitmap = new Bitmap(filePath);
            Validate(this._bitmap);
        }

        public ImageProcessor(Bitmap bitmap)
        {
            this._bitmap = bitmap;
            Validate(this._bitmap);
        }

        public ColorHistogramDescriptor GetChannel(ColorChannel colorChannel)
        {
            var result = new ColorHistogramDescriptor
            {
                ColorChannel = colorChannel,
                Distribution = this.GetChannelDistribution((int)colorChannel)
            };

            return result;
        }

        public void ApplyFilter(IFilter filter)
        {
            filter.Apply(this._bitmap);
        }

        private int[] GetChannelDistribution(int position)
        {
            var result = new int[256];

            var rect = new Rectangle(0, 0, this._bitmap.Width, this._bitmap.Height);
            BitmapData bitmapData = this._bitmap.LockBits(rect, ImageLockMode.ReadWrite, this._bitmap.PixelFormat);
            IntPtr startPtr = bitmapData.Scan0;

            int imageSize = Math.Abs(bitmapData.Stride) * this._bitmap.Height;
            var rgbBytes = new byte[imageSize];
            Marshal.Copy(startPtr, rgbBytes, 0, imageSize);

            for (int i = position; i < rgbBytes.Length; i += 3)
            {
                result[rgbBytes[i]] += 1;
            }

            Marshal.Copy(rgbBytes, 0, startPtr, imageSize);
            this._bitmap.UnlockBits(bitmapData);

            return result;
        }

        public void SaveChanges(string filePath)
        {
            this._bitmap.Save(filePath);
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        private static void Validate(Bitmap image)
        {
            if (image.PixelFormat != PixelFormat.Format24bppRgb)
            {
                throw new ArgumentException("The image should be of 24bppRgb format.");
            }
        }

        private void Dispose(bool disposing)
        {
            if (this._disposed == false)
            {
                if (disposing)
                {
                    this._bitmap.Dispose();
                    this._disposed = true;
                }
            }
        }
    }
}
