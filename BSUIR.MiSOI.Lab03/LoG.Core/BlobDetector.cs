using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Point = System.Drawing.Point;

namespace LoG.Core
{
  public sealed class BlobDetector : IBlobDetector
  {
    private const double InitialT = 1.0d;
    private const double Step = 0.1d;
    private const double FinalT = 3.0d;

    private readonly IFilterBuilder _filterBuilder;
    private readonly DataInitializer _dataInitializer;
    private readonly string _path;
    private readonly ImageWrapper _originalImage;
    private ScaleLevel[] _scaleSpace;
    private bool _initialized = false;

    public BlobDetector(IFilterBuilder filterBuilder, string path)
    {
      _filterBuilder = filterBuilder;
      _dataInitializer = new DataInitializer(path);
      _path = path;
      _originalImage = new ImageWrapper(new Bitmap(path));
      _scaleSpace = new ScaleLevel[(int)(((FinalT - InitialT) / Step) + 1)];
    }

    public void DetectBlobs(string resultFile)
    {
      this.CreateScaleSpace();
      var blobPartitioner = new BlobPartitioner(_scaleSpace);
      this.Save();
      IList<Blob> blobs = blobPartitioner.Process();
      this.ApplyBlobs(blobs);
    }

    private void ApplyBlobs(IList<Blob> blobs)
    {
      using (var bitmap = new Bitmap(this._path))
      {
        foreach (var blob in blobs)
        {
          IList<Point> points = blob.GetPoints();
          foreach (Point point in points)
          {
            if (point.X > 0 && point.X < bitmap.Height && point.Y > 0 && point.Y < bitmap.Width)
            {
              bitmap.SetPixel(point.Y, point.X, Color.Red);
            }
          }
        }

        bitmap.Save("res.jpg");
      }
    }

    private void CreateScaleSpace()
    {
      IList<ScaleLevel> scaleLevels = new List<ScaleLevel>();
      if (_dataInitializer.DataExists())
      {
        scaleLevels = _dataInitializer.ReadData(_filterBuilder);
      }

      if (scaleLevels.Count == 0)
      {
        Parallel.For(
          0,
          (int)(((FinalT - InitialT) / Step) + 1),
          (i, state) =>
            {
              IFilter filter = _filterBuilder.Build(InitialT + (i * Step));
              var scaleLevel = new ScaleLevel(filter, this._originalImage.GetDeepCopy());
              scaleLevel.ApplyTransform().FindBlobs();
              _scaleSpace[i] = scaleLevel;
            });
      }
      else
      {
        int number = (int)(((FinalT - InitialT) / Step) + 1);
        _scaleSpace = scaleLevels.Take(number).ToArray();
        Parallel.ForEach(_scaleSpace, (level) => level.FindBlobs());
        _initialized = true;
      }
      _scaleSpace = _scaleSpace.OrderBy(p => p.Sigma).ToArray();
    }

    private void Save()
    {
      if (_initialized == false)
      {
        for (int i = 0; i < _scaleSpace.Length; ++i)
        {
          _dataInitializer.Save(i.ToString(CultureInfo.InvariantCulture), _scaleSpace[i]);
        }
      }
    }
  }
}
