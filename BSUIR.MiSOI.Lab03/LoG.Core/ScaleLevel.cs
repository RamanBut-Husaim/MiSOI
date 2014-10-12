using System.Collections.Generic;

namespace LoG.Core
{
  internal sealed class ScaleLevel
  {
    private const double IntensityThreshold = 100;
    private const double SizeThreshold = 1000;
    private const double ShiftThreshold = 20;

    private readonly IFilter _filter;

    private readonly ImageWrapper _imageWrapper;

    private readonly IList<Blob> _blobs; 

    public ScaleLevel(IFilter filter, ImageWrapper imageWrapper)
    {
      _filter = filter;
      _imageWrapper = imageWrapper;
      _blobs = new List<Blob>();
    }

    public IList<Blob> Blobs
    {
      get { return _blobs; }
    } 

    public ImageWrapper Image
    {
      get { return _imageWrapper; }
    }

    public double Sigma
    {
      get { return ((LogFilter)_filter).Sigma; }
    }

    public double T
    {
      get { return ((LogFilter)_filter).T; }
    }

    public ScaleLevel ApplyTransform()
    {
      _filter.Apply(_imageWrapper);
      return this;
    }

    public void FindBlobs()
    {
      for (int i = 1; i < _imageWrapper.Height - 1; ++i)
      {
        for (int j = 1; j < _imageWrapper.Width - 1; ++j)
        {
          if (this.IsMaximum(i, j))
          {
            int y = j - (int)(this.T * ShiftThreshold);
            var blob = new Blob(i, y, ((LogFilter)_filter).Sigma, this.Image[i, j]);
            if (blob.Area > SizeThreshold)
            {
              this.Blobs.Add(blob);
            }
          }
        }
      }
    }

    private bool IsMaximum(int i, int j)
    {
      return this.Image[i - 1, j - 1] <= this.Image[i, j] && this.Image[i - 1, j] <= this.Image[i, j] && this.Image[i - 1, j + 1] <= this.Image[i, j]
             && this.Image[i, j - 1] <= this.Image[i, j] && this.Image[i, j + 1] <= this.Image[i, j] && this.Image[i + 1, j - 1] <= this.Image[i, j]
             && this.Image[i + 1, j] <= this.Image[i, j] && this.Image[i + 1, j + 1] <= this.Image[i, j] && this.Image[i, j] < IntensityThreshold;
    }
  }
}
