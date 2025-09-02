using System.Runtime.CompilerServices;
using ChunkIt.Common;
using ChunkIt.Common.Extensions;

namespace ChunkIt.Abstractions;

public sealed class ChunkReader
{
    public IPartitioner Partitioner { get; }
    public IHasher Hasher { get; }

    public int BufferSize { get; }

    public ChunkReader(IPartitioner partitioner, IHasher hasher, int bufferSize)
    {
        Partitioner = partitioner;
        Hasher = hasher;
        BufferSize = bufferSize;
    }

    public async IAsyncEnumerable<Chunk> ReadAsync(
        Stream stream,
        [EnumeratorCancellation] CancellationToken cancellationToken = default
    )
    {
        using var bufferLease = new ArrayPoolLease<byte>(BufferSize);

        var buffer = bufferLease.Memory;
        var remainderBuffer = Memory<byte>.Empty;

        var chunkOffset = 0L;
        var chunkId = 0L;

        while (true)
        {
            var inputBuffer = await stream.RefillAsync(buffer, remainderBuffer, cancellationToken);

            if (inputBuffer.IsEmpty)
            {
                yield break;
            }

            var chunkLength = Partitioner.FindChunkLength(inputBuffer.Span);
            (var chunkBuffer, remainderBuffer) = inputBuffer.Split(chunkLength);

            yield return new Chunk
            {
                Id = chunkId,
                Offset = chunkOffset,
                Length = chunkLength,
                Hash = Hasher.Hash(chunkBuffer.Span),
            };

            chunkOffset += chunkLength;
            chunkId++;
        }
    }

    public override string ToString()
    {
        var partitionerName = Partitioner.GetType().Name;
        var hasherName = Hasher.GetType().Name;

        return $"{partitionerName} {hasherName} {BufferSize}";
    }
}