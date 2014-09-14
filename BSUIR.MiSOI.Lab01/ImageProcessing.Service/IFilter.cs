using System.Drawing;

namespace ImageProcessing.Service
{
    public interface IFilter
    {
        void Apply(Bitmap bitMap);
    }
}
