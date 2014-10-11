using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoG.Core
{
  internal sealed class ScaleLevel
  {
    private readonly IFilter _filter;

    private readonly ImageWrapper _imageWrapper;

    public ScaleLevel(IFilter filter, ImageWrapper imageWrapper)
    {
      _filter = filter;
      _imageWrapper = imageWrapper;
    }

    public ImageWrapper Image
    {
      get { return _imageWrapper; }
    }

    public void ApplyTransform()
    {
      _filter.Apply(_imageWrapper);
    }
  }
}
