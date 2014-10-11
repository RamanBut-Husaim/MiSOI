using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace LoG.Core
{
  public sealed class BlobDetector : IBlobDetector
  {
    private const double InitialT = 1.0d;
    private const double Step = 0.1d;
    private const double FinalT = 3.0d;

    private readonly IFilterBuilder _filterBuilder;
    private readonly string _path;
    private readonly ImageWrapper _originalImage;
    private readonly ScaleLevel[] _scaleSpace; 

    public BlobDetector(IFilterBuilder filterBuilder, string path)
    {
      _filterBuilder = filterBuilder;
      _path = path;
      _originalImage = new ImageWrapper(new Bitmap(path));
      _scaleSpace = new ScaleLevel[(int)(((FinalT - InitialT) / Step) + 1)];
    }

    public void DetectBlobs(string resultFile)
    {
      this.CreateScaleSpace();
      this.Save();
    }

    private void CreateScaleSpace()
    {
      Parallel.For(
        0,
        (int)(((FinalT - InitialT) / Step) + 1),
        (i, state) =>
          {
            IFilter filter = _filterBuilder.Build(InitialT + (i * Step));
            var scaleLevel = new ScaleLevel(filter, this._originalImage.GetDeepCopy());
            scaleLevel.ApplyTransform();
            _scaleSpace[i] = scaleLevel;
          });
    }

    private void Save()
    {
      for (int i = 0; i < _scaleSpace.Length; ++i)
      {
        this.Save(i.ToString(CultureInfo.InvariantCulture) + ".jpg", _scaleSpace[i]);
      }
    }

    private void Save(string filePath, ScaleLevel scaleLevel)
    {
      using (var image = new Bitmap(_path))
      {
        var rect = new Rectangle(0, 0, image.Width, image.Height);
        BitmapData bitmapData = image.LockBits(rect, ImageLockMode.ReadWrite, image.PixelFormat);
        IntPtr startPtr = bitmapData.Scan0;

        int imageSize = Math.Abs(bitmapData.Stride) * image.Height;

        byte[] bytes = scaleLevel.Image.ToByteArray();

        Marshal.Copy(bytes, 0, startPtr, imageSize);
        image.UnlockBits(bitmapData);

        image.Save(Path.Combine("LoG", filePath));
      }
    }
  }
}
