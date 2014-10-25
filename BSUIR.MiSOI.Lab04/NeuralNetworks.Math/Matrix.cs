using System;

namespace NeuralNetworks.Math
{
  public sealed class Matrix
  {
    private const double Eps = 0.0000001;
    private readonly double[,] _matrix;

    public Matrix(bool[,] sourceMatrix)
    {
      this._matrix = new double[sourceMatrix.GetUpperBound(0) + 1, sourceMatrix.GetUpperBound(1) + 1];
      for (int r = 0; r < this.Rows; r++)
      {
        for (int c = 0; c < this.Cols; c++)
        {
          if (sourceMatrix[r, c])
          {
            this[r, c] = 1;
          }
          else
          {
            this[r, c] = -1;
          }
        }
      }
    }

    public Matrix(double[,] sourceMatrix)
    {
      this._matrix = new double[sourceMatrix.GetUpperBound(0) + 1, sourceMatrix.GetUpperBound(1) + 1];
      for (int r = 0; r < this.Rows; r++)
      {
        for (int c = 0; c < this.Cols; c++)
        {
          this[r, c] = sourceMatrix[r, c];
        }
      }
    }

    public Matrix(int rows, int cols)
    {
      this._matrix = new double[rows, cols];
    }

    public int Cols
    {
      get
      {
        return this._matrix.GetUpperBound(1) + 1;
      }
    }

    public int Rows
    {
      get
      {
        return this._matrix.GetUpperBound(0) + 1;
      }
    }

    public int Size
    {
      get
      {
        return this.Rows * this.Cols;
      }
    }

    public double this[int row, int col]
    {
      get
      {
        return this._matrix[row, col];
      }

      set
      {
        this._matrix[row, col] = value;
      }
    }

    public static Matrix CreateColumnMatrix(double[] input)
    {
      var d = new double[input.Length, 1];
      for (int row = 0; row < d.Length; row++)
      {
        d[row, 0] = input[row];
      }

      return new Matrix(d);
    }

    public static Matrix CreateRowMatrix(double[] input)
    {
      var d = new double[1, input.Length];

      for (int i = 0; i < input.Length; i++)
      {
        d[0, i] = input[i];
      }

      return new Matrix(d);
    }

    public void Add(int row, int col, double value)
    {
      double newValue = this[row, col] + value;
      this[row, col] = newValue;
    }

    public void Clear()
    {
      for (int r = 0; r < this.Rows; r++)
      {
        for (int c = 0; c < this.Cols; c++)
        {
          this[r, c] = 0;
        }
      }
    }

    public Matrix Clone()
    {
      return new Matrix(this._matrix);
    }

    public int FromPackedArray(double[] array, int index)
    {
      for (int r = 0; r < this.Rows; r++)
      {
        for (int c = 0; c < this.Cols; c++)
        {
          this._matrix[r, c] = array[index++];
        }
      }

      return index;
    }

    public Matrix GetCol(int col)
    {
      var newMatrix = new double[this.Rows, 1];

      for (int row = 0; row < this.Rows; row++)
      {
        newMatrix[row, 0] = this._matrix[row, col];
      }

      return new Matrix(newMatrix);
    }

    public Matrix GetRow(int row)
    {
      var newMatrix = new double[1, this.Cols];

      for (int col = 0; col < this.Cols; col++)
      {
        newMatrix[0, col] = this._matrix[row, col];
      }

      return new Matrix(newMatrix);
    }

    public bool IsVector()
    {
      if (this.Rows == 1)
      {
        return true;
      }

      return this.Cols == 1;
    }

    public bool IsZero()
    {
      for (int row = 0; row < this.Rows; row++)
      {
        for (int col = 0; col < this.Cols; col++)
        {
          if (System.Math.Abs(this._matrix[row, col]) > Matrix.Eps)
          {
            return false;
          }
        }
      }

      return true;
    }

    public void Ramdomize(double min, double max)
    {
      var rand = new Random();

      for (int r = 0; r < this.Rows; r++)
      {
        for (int c = 0; c < this.Cols; c++)
        {
          this._matrix[r, c] = (rand.NextDouble() * (max - min)) + min;
        }
      }
    }

    public double Sum()
    {
      double result = 0;
      for (int r = 0; r < this.Rows; r++)
      {
        for (int c = 0; c < this.Cols; c++)
        {
          result += this._matrix[r, c];
        }
      }

      return result;
    }

    public double[] ToPackedArray()
    {
      var result = new double[this.Rows * this.Cols];

      int index = 0;
      for (int r = 0; r < this.Rows; r++)
      {
        for (int c = 0; c < this.Cols; c++)
        {
          result[index++] = this._matrix[r, c];
        }
      }

      return result;
    }
  }
}
