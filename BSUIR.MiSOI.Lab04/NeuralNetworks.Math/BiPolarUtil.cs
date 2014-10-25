namespace NeuralNetworks.Math
{
  public sealed class BiPolarUtil
  {
    public static double Bipolar2Double(bool b)
    {
      if (b)
      {
        return 1;
      }

      return -1;
    }

    public static double[] Bipolar2Double(bool[] b)
    {
      var result = new double[b.Length];

      for (int i = 0; i < b.Length; i++)
      {
        result[i] = BiPolarUtil.Bipolar2Double(b[i]);
      }

      return result;
    }

    public static double[,] Bipolar2Double(bool[,] b)
    {
      var result = new double[b.GetUpperBound(0), b.GetUpperBound(1)];

      for (int row = 0; row < b.GetUpperBound(0); row++)
      {
        for (int col = 0; col < b.GetUpperBound(1); col++)
        {
          result[row, col] = BiPolarUtil.Bipolar2Double(b[row, col]);
        }
      }

      return result;
    }

    public static bool Double2Bipolar(double d)
    {
      if (d > 0)
      {
        return true;
      }

      return false;
    }

    public static bool[] Double2Bipolar(double[] d)
    {
      var result = new bool[d.Length];

      for (int i = 0; i < d.Length; i++)
      {
        result[i] = BiPolarUtil.Double2Bipolar(d[i]);
      }

      return result;
    }

    public static bool[,] Double2Bipolar(double[,] d)
    {
      var result = new bool[d.GetUpperBound(0), d.GetUpperBound(1)];

      for (int row = 0; row < d.GetUpperBound(0); row++)
      {
        for (int col = 0; col < d.GetUpperBound(0); col++)
        {
          result[row, col] = BiPolarUtil.Double2Bipolar(d[row, col]);
        }
      }

      return result;
    }

    public static double NormalizeBinary(double d)
    {
      if (d > 0)
      {
        return 1;
      }

      return 0;
    }

    public static double ToBinary(double d)
    {
      return (d + 1) / 2.0;
    }

    public static double ToBiPolar(double d)
    {
      return (2 * BiPolarUtil.NormalizeBinary(d)) - 1;
    }

    public static double ToNormalizedBinary(double d)
    {
      return BiPolarUtil.NormalizeBinary(BiPolarUtil.ToBinary(d));
    }
  }
}
