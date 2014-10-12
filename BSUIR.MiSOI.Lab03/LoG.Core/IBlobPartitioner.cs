using System.Collections.Generic;

namespace LoG.Core
{
  public interface IBlobPartitioner
  {
    IList<Blob> Process();
  }
}
