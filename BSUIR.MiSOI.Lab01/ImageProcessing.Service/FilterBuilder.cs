namespace ImageProcessing.Service
{
    public sealed class FilterBuilder : IFilterBuilder
    {
        public IFilter CreateSolarizationFilter(double k)
        {
            return new SolarizationFilter(k);
        }

        public IFilter CreateStampingFilter()
        {
            return new StampingFilter();
        }

        public IFilter CreateStampingLinearFilter()
        {
            return new LinearStampingFilter();
        }

        public IFilter CreateBinaryHalftoneFilter()
        {
            return new BinaryHalftoneFilter();
        }
    }
}
