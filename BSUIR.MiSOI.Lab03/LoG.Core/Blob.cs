using System;
using System.Collections.Generic;
using System.Drawing;

namespace LoG.Core
{
  public struct Blob
  {
    private const int PointNumber = 200;
    private readonly int _x;
    private readonly int _y;
    private readonly double _sigma;
    private readonly byte _intensity;

    public Blob(int x, int y, double sigma, byte intensity)
    {
      this._x = x;
      this._y = y;
      this._sigma = sigma;
      this._intensity = intensity;
    }

    public int X
    {
      get { return _x; }
    }

    public int Y
    {
      get { return _y; }
    }

    public double Sigma
    {
      get { return _sigma; }
    }

    public double Radius
    {
      get { return this.Sigma * 1.5d; }
    }

    public byte Intensity
    {
      get { return _intensity; }
    }

    public double Area
    {
      get { return Math.PI * this.Radius * this.Radius; }
    }

    public IList<Point> GetPoints()
    {
      double step = Math.PI * 2 / PointNumber;
      IList<Point> result = new List<Point>(PointNumber);
      for (double angle = 0.0d; angle < 2 * Math.PI; angle += step)
      {
        int x = this.X + (int)(this.Radius * Math.Cos(angle));
        int y = this.Y + (int)(this.Radius * Math.Sin(angle));
        result.Add(new Point(x, y));
      }

      return result;
    } 
  }
}
