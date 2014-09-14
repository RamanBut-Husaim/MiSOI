using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ImageProcessing.Service
{
    internal abstract class LinearFilterBase : FilterBase
    {
        public override void Apply(Bitmap bitMap)
        {
            var rect = new Rectangle(0, 0, bitMap.Width, bitMap.Height);
            BitmapData bitmapData = bitMap.LockBits(rect, ImageLockMode.ReadWrite, bitMap.PixelFormat);
            IntPtr startPtr = bitmapData.Scan0;

            int imageSize = Math.Abs(bitmapData.Stride) * bitMap.Height;
            var rgbBytes = new byte[imageSize];
            Marshal.Copy(startPtr, rgbBytes, 0, imageSize);

            this.ApplyInternal(rgbBytes, Math.Abs(bitmapData.Stride));

            Marshal.Copy(rgbBytes, 0, startPtr, imageSize);
            bitMap.UnlockBits(bitmapData);
        }

        protected abstract void ApplyInternal(byte[] imageBytes, int rowWidth);
    }
}
