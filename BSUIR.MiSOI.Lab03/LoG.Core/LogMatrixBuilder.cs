using System;

namespace LoG.Core
{
  public sealed class LogMatrixBuilder : ILogMatrixBuilder
  {
    private double _sigma;

    private double[][] _logMatrix;

    private static double Norm(double x, double y)
    {
      return Math.Sqrt((x * x) + (y * y));
    }

    private static double Distance(double x1, double y1, double x2, double y2)
    {
      return Norm(x1 - x2, y1 - y2);
    }

    public double[][] Build(double sigma)
    {
      _sigma = sigma;

      int matrixSize = (int)Math.Ceiling(6 * _sigma);
      matrixSize = matrixSize % 2 == 0 ? matrixSize + 1 : matrixSize;

      int width = matrixSize / 2 + 1;
      //var width = (int)((3.35 * _sigma) + 0.33);
      //int matrixSize = width + width + 1;

      this.CreateLogMatrix(matrixSize);
      for (int i = 0; i < matrixSize; ++i)
      {
        for (int j = 0; j < matrixSize; ++j)
        {
          _logMatrix[i][j] = this.LoG(Distance(i, j, width, width));
        }
      }

      return _logMatrix;
    }

    private void CreateLogMatrix(int size)
    {
      _logMatrix = new double[size][];
      for (int i = 0; i < size; ++i)
      {
        _logMatrix[i] = new double[size];
      }
    }

    private double Gauss(double x)
    {
      return Math.Exp((-x * x) / (2 * _sigma * _sigma));
    }

    private double LoG(double x)
    {
      //return (-1 / (Math.PI * _sigma * _sigma * _sigma * _sigma)) * (1 - ((x * x) / (2 * _sigma * _sigma))) * this.Gauss(x);
      return (((x * x) - (2 * _sigma * _sigma)) / (2 * Math.PI * Math.Pow(_sigma, 4))) * this.Gauss(x);
      //return ((x * x) - (2 * _sigma * _sigma)) * this.Gauss(x);
    }
  }
}
