using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace NeuralNetworks.Hopfield.Core
{
  public sealed class ImageClassifier
  {
    private readonly HopfieldNetwork _hopfieldNetwork;
    private readonly IImageBinarizer _imageBinarizer;
    private readonly IList<TrainPattern> _trainPatterns; 

    public ImageClassifier(IImageBinarizer imageBinazer, int size)
    {
      _hopfieldNetwork = new HopfieldNetwork(size);
      _trainPatterns = new List<TrainPattern>();
      _imageBinarizer = imageBinazer;
    }

    public void Train(ImageUnit imageUnit)
    {
      var trainPattern = new TrainPattern(imageUnit) { BinarizedRepresentation = _imageBinarizer.ApplyBinarization(imageUnit.Image) };
      this._trainPatterns.Add(trainPattern);
      _hopfieldNetwork.Train(trainPattern.BinarizedRepresentation);
    }

    public string Classify(Bitmap image)
    {
      string result = string.Empty;
      bool[] binarizedImage = _imageBinarizer.ApplyBinarization(image);
      bool[] trainPattern = _hopfieldNetwork.Present(binarizedImage);
      foreach (var pattern in _trainPatterns)
      {
        if (trainPattern.SequenceEqual(pattern.BinarizedRepresentation))
        {
          result = pattern.Id;
          break;
        }
      }

      return result;
    }
  }
}
