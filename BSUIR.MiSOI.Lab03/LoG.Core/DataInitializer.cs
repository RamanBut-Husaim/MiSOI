using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;

namespace LoG.Core
{
  internal sealed class DataInitializer
  {
    private const string FolderName = "LoG";
    private const string Extension = ".logm";
    private readonly string _path;

    public DataInitializer(string path)
    {
      this._path = path;
    }

    public bool DataExists()
    {
      var result = false;

      if (Directory.Exists(FolderName) == false)
      {
        return false;
      }

      foreach (var path in Directory.EnumerateFiles(FolderName))
      {
        var fileInfo = new FileInfo(path);
        result = Extension.Equals(fileInfo.Extension, StringComparison.OrdinalIgnoreCase);
        if (result)
        {
          return true;
        }
      }

      return false;
    }

    public IList<ScaleLevel> ReadData(IFilterBuilder filterBuilder)
    {
      var result = new List<ScaleLevel>();
      foreach (var path in Directory.EnumerateFiles(FolderName))
      {
        var fileInfo = new FileInfo(path);
        if (Extension.Equals(fileInfo.Extension, StringComparison.OrdinalIgnoreCase))
        {
          using (var stream = fileInfo.OpenRead())
          {
            var deserilizer = new BinaryFormatter();
            var metadata = (ScaleMetadata)deserilizer.Deserialize(stream);
            var scaleLevel = new ScaleLevel(filterBuilder.Build(metadata.T), new ImageWrapper(new Bitmap(metadata.FileName)));
            result.Add(scaleLevel);
          }
        }
      }

      return result;
    }

    public void Save(string filePath, ScaleLevel scaleLevel)
    {
      if (filePath.Length == 1)
      {
        filePath = "0" + filePath;
      }

      this.SaveImage(filePath, scaleLevel);
      this.SaveMetadata(filePath, scaleLevel);
    }

    private void SaveImage(string filePath, ScaleLevel scaleLevel)
    {
      using (var image = new Bitmap(_path))
      {
        var rect = new Rectangle(0, 0, image.Width, image.Height);
        BitmapData bitmapData = image.LockBits(rect, ImageLockMode.ReadWrite, image.PixelFormat);
        IntPtr startPtr = bitmapData.Scan0;

        int imageSize = Math.Abs(bitmapData.Stride) * image.Height;

        byte[] bytes = scaleLevel.Image.ToByteArray();

        Marshal.Copy(bytes, 0, startPtr, imageSize);
        image.UnlockBits(bitmapData);

        image.Save(Path.Combine("LoG", filePath + ".jpg"));
      }
    }

    private void SaveMetadata(string filePath, ScaleLevel scaleLevel)
    {
      using (var file = new FileStream(Path.Combine("Log", filePath + Extension), FileMode.Create, FileAccess.Write, FileShare.None))
      {
        var serializer = new BinaryFormatter();
        var obj = new ScaleMetadata { FileName = Path.Combine("LoG", filePath + ".jpg"), T = scaleLevel.T };
        serializer.Serialize(file, obj);
      }
    }
  }
}
