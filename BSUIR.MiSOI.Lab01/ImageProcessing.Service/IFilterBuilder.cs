namespace ImageProcessing.Service
{
    public interface IFilterBuilder
    {
        IFilter CreateSolarizationFilter(double k);

        IFilter CreateStampingFilter();

        IFilter CreateStampingLinearFilter();

        IFilter CreateBinaryHalftoneFilter();
    }
}
