using System.Drawing;

namespace ImageProcessing.Service
{
    public abstract class FilterBase : IFilter
    {
        protected const double RFactor = 0.3d;

        protected const double BFactor = 0.11d;

        protected const double GFactor = 0.59d;

        public abstract void Apply(Bitmap bitMap);
    }
}
