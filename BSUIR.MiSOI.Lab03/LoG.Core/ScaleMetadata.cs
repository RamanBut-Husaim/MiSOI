using System;

namespace LoG.Core
{
  [Serializable]
  internal sealed class ScaleMetadata
  {
    public double T { get; set; }
    public string FileName { get; set; }
  }
}
