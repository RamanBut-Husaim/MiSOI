using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using NeuralNetworks.Hopfield.Core;

namespace Hopfield.Runner
{
  public sealed class Program
  {
    private const string DataDirectoryName = "Data";
    private const int Size = 10;

    public static void Main(string[] args)
    {
      var statistics = new Dictionary<string, int>();

      IImageBinarizer binarizer = new ImageBinarizer();
      var directoryInfo = new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.Parent;
      var path = Path.Combine(directoryInfo.FullName, Program.DataDirectoryName);
      directoryInfo = new DirectoryInfo(path);

      var imageClassifier = new ImageClassifier(binarizer, Program.Size * Program.Size);
      //Train
      foreach (DirectoryInfo directory in directoryInfo.EnumerateDirectories())
      {
        foreach (var file in directory.EnumerateFiles("*.bmp"))
        {
          string fileName = Path.GetFileNameWithoutExtension(file.FullName);
          imageClassifier.Train(new ImageUnit { Id = fileName, Image = new Bitmap(file.FullName) });
        }
      }

      foreach (DirectoryInfo directory in directoryInfo.EnumerateDirectories())
      {
        foreach (var file in directory.EnumerateFiles("*.bmp"))
        {
          string fileName = Path.GetFileNameWithoutExtension(file.FullName);
          var patternDirectory = directory.EnumerateDirectories().FirstOrDefault(p => p.Name == ("Randomized_" + fileName));
          foreach (var patternFile in patternDirectory.EnumerateFiles("*.bmp"))
          {
            string patternFileName = Path.GetFileNameWithoutExtension(patternFile.FullName);
            string[] nameParts = patternFileName.Split(new[] { "_" }, StringSplitOptions.RemoveEmptyEntries);
            string percentValue = nameParts[1];
            if (statistics.ContainsKey(percentValue) == false)
            {
              statistics.Add(percentValue, 0);
            }
            string classificationResult = imageClassifier.Classify(new Bitmap(patternFile.FullName));
            if (classificationResult.Equals(fileName, StringComparison.CurrentCulture))
            {
              statistics[percentValue] += 1;
            }
          }
        }
      }

      Console.WriteLine("Classification result: ");

      foreach (var statistic in statistics)
      {
        Console.WriteLine("Percent - {0} Match - {1}", statistic.Key, statistic.Value);
      }

      Console.ReadLine();
    }
  }
}
