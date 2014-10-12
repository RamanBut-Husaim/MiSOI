using System;
using System.Collections.Generic;

namespace LoG.Core
{
  internal sealed class BlobPartition
  {
    private const double OverlapThreshold = 0.05d;
    private readonly IList<Blob> _blobs;
    private readonly IList<Blob> _result;
    private readonly bool[] _kept;

    public BlobPartition(IList<Blob> blobs)
    {
      this._blobs = blobs;
      this._result = new List<Blob>();
      this._kept = new bool[blobs.Count].WithInitialValue(true);
    }

    public IList<Blob> Result
    {
      get { return _result; }
    } 

    public void Process()
    {
      for (int i = 0; i < _blobs.Count - 1; ++i)
      {
        for (int j = i + 1; j < _blobs.Count; ++j)
        {
          if (_kept[i] == false || _kept[j] == false)
          {
            continue;
          }

          double centerDistance = Distance(_blobs[i].X, _blobs[i].Y, _blobs[j].X, _blobs[j].Y);
          double r1Sqaure = _blobs[i].Radius * _blobs[i].Radius;
          double dSquare = centerDistance * centerDistance;
          double r2Square = _blobs[j].Radius * _blobs[j].Radius;

          //See http://mathworld.wolfram.com/Circle-CircleIntersection.html
          double overlapArea = r1Sqaure * Math.Acos((dSquare + r1Sqaure - r2Square) / (2 * centerDistance * _blobs[i].Radius))
                               + r2Square * Math.Acos((dSquare + r2Square - r1Sqaure) / (2 * centerDistance * _blobs[j].Radius))
                               - 0.5
                               * Math.Sqrt(
                                 (-centerDistance + _blobs[i].Radius + _blobs[j].Radius) * (centerDistance + _blobs[i].Radius - _blobs[j].Radius)
                                 * (centerDistance - _blobs[i].Radius + _blobs[j].Radius) * (centerDistance + _blobs[i].Radius + _blobs[j].Radius));
          double overlapPercent = Math.Max(overlapArea / _blobs[i].Area, overlapArea / _blobs[j].Area);
          if (overlapPercent > OverlapThreshold)
          {
            if (_blobs[i].Intensity > _blobs[j].Intensity)
            {
              _kept[i] = true;
              _kept[j] = false;
            }
            else
            {
              _kept[i] = false;
              _kept[j] = true;
            }
          }
        }
      }

      for (int i = 0; i < _blobs.Count; ++i)
      {
        if (_kept[i])
        {
          _result.Add(_blobs[i]);
        }
      }
    }

    private static double Norm(double x, double y)
    {
      return Math.Sqrt((x * x) + (y * y));
    }

    private static double Distance(double x1, double y1, double x2, double y2)
    {
      return Norm(x1 - x2, y1 - y2);
    }
  }
}
