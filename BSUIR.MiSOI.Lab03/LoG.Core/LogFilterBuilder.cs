namespace LoG.Core
{
  public sealed class LogFilterBuilder : IFilterBuilder
  {
    public IFilter Build(double t)
    {
      return new LogFilter(t, new LogMatrixBuilder());
    }
  }
}
