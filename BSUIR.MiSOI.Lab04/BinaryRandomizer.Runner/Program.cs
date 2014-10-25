using System;
using System.IO;

namespace BinaryRandomizer.Runner
{
  internal sealed class Program
  {
    private const string DataDirectoryName = "Data";

    public static void Main(string[] args)
    {
      var directoryInfo = new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.Parent;
      var path = Path.Combine(directoryInfo.FullName, Program.DataDirectoryName);
      directoryInfo = new DirectoryInfo(path);
      foreach (var directory in directoryInfo.EnumerateDirectories())
      {
        foreach (var file in directory.EnumerateFiles("*.bmp"))
        {
          var randomizer = new BinaryRandimizer.Core.BinaryRandomizer(file.FullName);
          randomizer.ApplyRandomization();
        }
      }
    }
  }
}
