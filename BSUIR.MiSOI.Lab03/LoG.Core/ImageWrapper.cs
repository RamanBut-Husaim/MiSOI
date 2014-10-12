using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace LoG.Core
{
  [Serializable]
  public sealed class ImageWrapper
  {
    private double[][] _imageData;

    private int _width;

    private int _height;

    private int _stride;

    public ImageWrapper(Bitmap image)
    {
      this.Initialize(image);
    }

    private ImageWrapper(ImageWrapper other)
    {
      _width = other.Width;
      _height = other.Height;
      _stride = other._stride;
      _imageData = new double[other.Height][];

      for (int i = 0; i < this.Height; ++i)
      {
        _imageData[i] = new double[other.Width];
        for (int j = 0; j < this.Width; ++j)
        {
          _imageData[i][j] = other._imageData[i][j];
        }
      }
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

    public double this[int i, int j]
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
          result[(i * this._stride) + (j * 3)] = (byte)Math.Abs(this._imageData[i][j]);
          result[(i * this._stride) + ((j * 3) + 1)] = (byte)Math.Abs(this._imageData[i][j]);
          result[(i * this._stride) + ((j * 3) + 2)] = (byte)Math.Abs(this._imageData[i][j]);
        }
      }

      return result;
    }

    public ImageWrapper GetDeepCopy()
    {
      return new ImageWrapper(this);
    }

    private void Initialize(Bitmap bitMap)
    {
      var rect = new Rectangle(0, 0, bitMap.Width, bitMap.Height);
      BitmapData bitmapData = bitMap.LockBits(rect, ImageLockMode.ReadOnly, bitMap.PixelFormat);
      IntPtr startPtr = bitmapData.Scan0;

      int imageSize = Math.Abs(bitmapData.Stride) * bitMap.Height;
      var rgbBytes = new byte[imageSize];
      Marshal.Copy(startPtr, rgbBytes, 0, imageSize);

      this._imageData = new double[bitMap.Height][];
      this._width = bitMap.Width;
      this._height = bitMap.Height;
      this._stride = bitmapData.Stride;

      for (int i = 0; i < bitMap.Height; ++i)
      {
        this._imageData[i] = new double[bitMap.Width];
        for (int j = 0; j < bitMap.Width; ++j)
        {
          byte b = rgbBytes[(i * bitmapData.Stride) + (j * 3)];
          byte g = rgbBytes[(i * bitmapData.Stride) + ((j * 3) + 1)];
          byte r = rgbBytes[(i * bitmapData.Stride) + ((j * 3) + 2)];
          this._imageData[i][j] = ((int)r + (int)g + (int)b) / 3.0d;
        }
      }

      bitMap.UnlockBits(bitmapData);
    }
  }
}
