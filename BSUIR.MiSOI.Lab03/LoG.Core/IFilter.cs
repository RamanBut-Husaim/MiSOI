using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoG.Core
{
  public interface IFilter
  {
    void Apply(ImageWrapper imageWrapper);
  }
}
