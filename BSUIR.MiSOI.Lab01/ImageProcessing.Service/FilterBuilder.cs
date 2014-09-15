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

        public IFilter CreateGammaFilter(double c, double exp)
        {
            return new GammaFilter(c, exp);
        }

        public IFilter CreateLogarithmicFilter(double c)
        {
            return new LogarithmicFilter(c);
        }
    }
}
