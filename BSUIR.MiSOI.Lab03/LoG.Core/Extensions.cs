using System.Collections.Generic;

namespace LoG.Core
{
  internal static class Extensions
  {
    public static T[] WithInitialValue<T>(this T[] @this, T value)
    {
      for (int i = 0; i < @this.Length; ++i)
      {
        @this[i] = value;
      }

      return @this;
    } 
  }
}
