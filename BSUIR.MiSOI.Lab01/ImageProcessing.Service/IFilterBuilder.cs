namespace ImageProcessing.Service
{
    public interface IFilterBuilder
    {
        IFilter CreateSolarizationFilter(double k);

        IFilter CreateStampingFilter();

        IFilter CreateStampingLinearFilter();

        IFilter CreateBinaryHalftoneFilter();

        IFilter CreateGammaFilter(double c, double exp);

        IFilter CreateLogarithmicFilter(double c);
    }
}
