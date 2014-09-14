namespace ImageProcessing.Service
{
    public sealed class ColorHistogramDescriptor
    {
        public ColorChannel ColorChannel { get; internal set; }

        public int[] Distribution { get; internal set; }
    }
}
