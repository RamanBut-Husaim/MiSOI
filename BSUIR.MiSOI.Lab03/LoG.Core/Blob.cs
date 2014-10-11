namespace LoG.Core
{
  internal struct Blob
  {
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

    public byte Intensity
    {
      get { return _intensity; }
    }
  }
}
