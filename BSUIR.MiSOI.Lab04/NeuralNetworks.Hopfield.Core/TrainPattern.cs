namespace NeuralNetworks.Hopfield.Core
{
  internal sealed class TrainPattern : ImageUnit
  {
    public TrainPattern()
    {  
    }

    public TrainPattern(ImageUnit unit)
    {
      this.Id = unit.Id;
      this.Image = unit.Image;
    }

    public bool[] BinarizedRepresentation { get; set; }
  }
}
