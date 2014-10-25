using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace BinaryRandimizer.Core
{
  public sealed class BinaryTransformer : IBinaryTransformer, IDisposable
  {
    private readonly int _percent;
    private readonly RNGCryptoServiceProvider _rngCryptoServiceProvider;
    private readonly HashSet<int> _randomizationIndices; 
    private bool _disposed;

    public BinaryTransformer(int randomizationPercent)
    {
      _percent = randomizationPercent;
      _rngCryptoServiceProvider = new RNGCryptoServiceProvider();
      _randomizationIndices = new HashSet<int>();
    }

    public void ApplyRandomization(Bitmap bitMap)
    {
      var rect = new Rectangle(0, 0, bitMap.Width, bitMap.Height);
      BitmapData bitmapData = bitMap.LockBits(rect, ImageLockMode.ReadWrite, bitMap.PixelFormat);
      IntPtr startPtr = bitmapData.Scan0;

      int imageSize = Math.Abs(bitmapData.Stride) * bitMap.Height;
      var rgbBytes = new byte[imageSize];
      Marshal.Copy(startPtr, rgbBytes, 0, imageSize);

      this.InitializeRandomizationIndices(bitMap.Width * bitMap.Height);
      this.Invert(rgbBytes);

      Marshal.Copy(rgbBytes, 0, startPtr, imageSize);
      bitMap.UnlockBits(bitmapData);
    }

    private void InitializeRandomizationIndices(int size)
    {
      int indicesToInvert = size * _percent / 100;
      while (_randomizationIndices.Count < indicesToInvert)
      {
        var buffer = new byte[sizeof(int)];
        _rngCryptoServiceProvider.GetBytes(buffer);
        int newIndex = Math.Abs(buffer.ToInt()) % size;
        _randomizationIndices.Add(newIndex);
      }
    }

    private void Invert(byte[] image)
    {
      foreach (int index in _randomizationIndices)
      {
        image[index * 3] = (byte)(byte.MaxValue - image[index * 3]);
        image[(index * 3) + 1] = (byte)(byte.MaxValue - image[(index * 3) + 1]);
        image[(index * 3) + 2] = (byte)(byte.MaxValue - image[(index * 3) + 2]);
      }
    }

    public void Dispose()
    {
      this.Dispose(true);
    }

    private void Dispose(bool disposing)
    {
      if (_disposed == false)
      {
        if (disposing)
        {
          _rngCryptoServiceProvider.Dispose();
          _disposed = true;
        }
      }
    }
  }
}
