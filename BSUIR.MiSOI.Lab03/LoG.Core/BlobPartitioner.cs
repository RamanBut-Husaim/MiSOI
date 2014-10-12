using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoG.Core
{
  internal sealed class BlobPartitioner : IBlobPartitioner
  {
    private static readonly int ChunkCount = Environment.ProcessorCount;

    private readonly IList<ScaleLevel> _scaleSpace;

    public BlobPartitioner(IList<ScaleLevel> scaleSpace)
    {
      this._scaleSpace = scaleSpace;
    }

    public IList<Blob> Process()
    {
      int blobCount = _scaleSpace.Sum(p => p.Blobs.Count);
      var blobs = new List<Blob>(blobCount);

      foreach (var scale in _scaleSpace)
      {
        blobs.AddRange(scale.Blobs);
      }

      var blobPartition = new BlobPartition(blobs);
      blobPartition.Process();

      return blobPartition.Result.OrderByDescending(p => p.Area).ToList();
    } 

    //public IList<Blob> Process()
    //{
    //  int blobCount = _scaleSpace.Sum(p => p.Blobs.Count);
    //  var blobs = new List<Blob>(blobCount);

    //  foreach (var scale in _scaleSpace)
    //  {
    //    blobs.AddRange(scale.Blobs);  
    //  }

    //  int chunkSize = blobCount / ChunkCount;
    //  chunkSize = chunkSize != 0 ? chunkSize : blobs.Count;
    //  int parallelism = 0;
    //  do
    //  {
    //    IList<BlobPartition> partitions = new List<BlobPartition>();

    //    for (int i = 0, takenBlobs = 0; takenBlobs < blobs.Count; ++i)
    //    {
    //      List<Blob> chunkBlobs = blobs.Skip(i * chunkSize).Take(chunkSize).ToList();
    //      takenBlobs += chunkBlobs.Count;
    //      partitions.Add(new BlobPartition(chunkBlobs));
    //    }

    //    Parallel.ForEach(partitions, partition => partition.Process());
    //    parallelism = (partitions.Count / ChunkCount) + 1;
    //    blobs.Clear();
    //    foreach (var blobPartition in partitions)
    //    {
    //      blobs.AddRange(blobPartition.Result);
    //    }
        
    //    chunkSize = blobs.Count / parallelism;
    //  }
    //  while (parallelism > 1);

    //  var finalPartition = new BlobPartition(blobs);
    //  finalPartition.Process();
    //  blobs = finalPartition.Result.OrderByDescending(p => p.Area).ToList();

    //  return blobs;
    //}
  }
}
