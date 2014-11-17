using System;

namespace LoG.Core
{
  public sealed class LogFilter : IFilter
  {
    private readonly double _t;

    private readonly double _sigma;

    private readonly ILogMatrixBuilder _logMatrixBuilder;

    private double[][] _logMatrix;

    public LogFilter(double t, ILogMatrixBuilder logMatrixBuilder)
    {
      _t = t;
      _sigma = Math.Exp(_t);
      _logMatrixBuilder = logMatrixBuilder;
    }

    public double T
    {
      get { return _t; }
    }

    public double Sigma
    {
      get { return _sigma; }
    }

    public void Apply(ImageWrapper imageWrapper)
    {
      _logMatrix = _logMatrixBuilder.Build(this.Sigma);
      this.Convolve(imageWrapper);
    }

    private void Convolve(ImageWrapper imageWrapper)
    {
      ImageWrapper imageCopy = imageWrapper.GetDeepCopy();
      int halfSize = _logMatrix[0].Length / 2;
      int logMatrixSize = _logMatrix[0].Length;
      for (int i = 0; i < imageWrapper.Height; ++i)
      {
        for (int j = 0; j < imageWrapper.Width; ++j)
        {
          double value = 0.0;
          for (int ii = 0; ii < logMatrixSize; ++ii)
          {
            int n = i - halfSize + ii;
            if (n < 0 || n >= imageWrapper.Height)
            {
              continue;
            }

            for (int jj = 0; jj < logMatrixSize; ++jj)
            {
              int m = j - halfSize + jj;
              if (m < 0 || m >= imageWrapper.Width)
              {
                continue;
              }

              value += _logMatrix[ii][jj] * imageCopy[n, m];
            }
          }

          imageWrapper[i, j] = value;
        }
      }
    }
  }
}
