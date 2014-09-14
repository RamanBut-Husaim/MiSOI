using System;

namespace ImageProcessing.Service
{
    public interface IImageProcessor : IDisposable
    {
        ColorHistogramDescriptor GetChannel(ColorChannel colorChannel);

        void ApplyFilter(IFilter filter);
    }
}
