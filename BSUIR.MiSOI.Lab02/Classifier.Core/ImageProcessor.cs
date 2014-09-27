using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;

namespace Classifier.Core
{
    public sealed class ImageProcessor : IImageProcessor, IDisposable
    {
        private const int AreaTreshold = 400;
        private readonly Bitmap _image;
        private readonly ImageWrapper _imageWrapper;
        private int _backgroundColor;
        private bool _disposed;

        public ImageProcessor(Bitmap image)
        {
            this._image = image;
            this._imageWrapper = new ImageWrapper(image);
        }

        public ImageProcessor(string fileName)
        {
            this._image = new Bitmap(fileName);
            this._imageWrapper = new ImageWrapper(this._image);
        }

        public void ApplyBinarization(IImageBinarizer imageBinarizer)
        {
            imageBinarizer.Process(this._imageWrapper);
        }

        public IList<Polygon> ApplyLabeling(ILabelingService labelingService)
        {
            IList<Polygon> polygons = labelingService.Process(this._imageWrapper);
            this.InitializeBackgroundColor();
            this.ColorPolygons(polygons);
            return polygons.Where(p => p.Pixels.Count > AreaTreshold).ToList();
        }

        private void InitializeBackgroundColor()
        {
            int blackPixels = this._imageWrapper.GetColoredPixels(0);
            int whitePixels = this._imageWrapper.GetColoredPixels(byte.MaxValue);
            this._backgroundColor = Math.Max(blackPixels, whitePixels) == blackPixels ? 0 : byte.MaxValue;
        }

        private void ColorPolygons(IList<Polygon> polygons)
        {
            foreach (var polygon in polygons)
            {
                if (polygon.Pixels.Count > AreaTreshold)
                {
                    foreach (var pixel in polygon.Pixels)
                    {
                        this._imageWrapper[pixel.X, pixel.Y] = (byte)(pixel.Color - (polygon.Index * 30));
                    }
                }
                else
                {
                    foreach (var pixel in polygon.Pixels)
                    {
                        this._imageWrapper[pixel.X, pixel.Y] = (byte)this._backgroundColor;
                    }
                }
            }
        }

        public void Save(string filePath)
        {
            var rect = new Rectangle(0, 0, this._image.Width, this._image.Height);
            BitmapData bitmapData = this._image.LockBits(rect, ImageLockMode.ReadWrite, this._image.PixelFormat);
            IntPtr startPtr = bitmapData.Scan0;

            int imageSize = Math.Abs(bitmapData.Stride) * this._image.Height;

            byte[] bytes = this._imageWrapper.ToByteArray();

            Marshal.Copy(bytes, 0, startPtr, imageSize);
            this._image.UnlockBits(bitmapData);

            this._image.Save(filePath);
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (this._disposed == false)
            {
                if (disposing)
                {
                    this._image.Dispose();
                    this._disposed = true;
                }
            }
        }
    }
}
