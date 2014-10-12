using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using LoG.Core;

namespace LoG.Runner
{
    public sealed class Program
    {
        public static void Main(string[] args)
        {
          if (Directory.Exists("LoG") == false)
          {
            Directory.CreateDirectory("LoG");
          }

          IFilterBuilder filterBuilder = new LogFilterBuilder();
          IBlobDetector blobDetector = new BlobDetector(filterBuilder, "sunflower3.jpg");
          blobDetector.DetectBlobs("res.jpg");
        }
    }
}
