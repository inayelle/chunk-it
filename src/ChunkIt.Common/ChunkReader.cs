using System.Runtime.CompilerServices;
using ChunkIt.Common.Abstractions;
using ChunkIt.Common.Extensions;

namespace ChunkIt.Common;

public sealed class ChunkReader
{
    private readonly IPartitioner _partitioner;
    private readonly IHasher _hasher;

    private readonly int _bufferSize;

    public ChunkReader(IPartitioner partitioner, IHasher hasher, int bufferSize)
    {
        _partitioner = partitioner;
        _hasher = hasher;
        _bufferSize = bufferSize;
    }

    public async IAsyncEnumerable<Chunk> ReadChunksAsync(
        Stream stream,
        [EnumeratorCancellation] CancellationToken cancellationToken = default
    )
    {
        using var bufferLease = new ArrayPoolLease<byte>(_bufferSize);

        var buffer = bufferLease.Memory;
        var inputBuffer = Memory<byte>.Empty;
        var remainderBuffer = Memory<byte>.Empty;

        var chunkOffset = 0L;
        var chunkId = 0L;

        while (true)
        {
            if (remainderBuffer.Length >= _partitioner.MaximumChunkSize)
            {
                inputBuffer = remainderBuffer;
            }
            else
            {
                inputBuffer = await stream.RefillAsync(buffer, remainderBuffer, cancellationToken);
            }

            if (inputBuffer.IsEmpty)
            {
                yield break;
            }

            var chunkLength = _partitioner.FindChunkLength(inputBuffer.Span);
            (var chunkBuffer, remainderBuffer) = inputBuffer.Split(chunkLength);

            yield return new Chunk
            {
                Id = chunkId,
                Offset = chunkOffset,
                Length = chunkLength,
                Hash = _hasher.Hash(chunkBuffer.Span),
            };

            chunkOffset += chunkLength;
            chunkId++;
        }
    }

    public async IAsyncEnumerable<int> ReadChunkLengthsAsync(
        Stream stream,
        [EnumeratorCancellation] CancellationToken cancellationToken = default
    )
    {
        using var bufferLease = new ArrayPoolLease<byte>(_bufferSize);

        var buffer = bufferLease.Memory;
        var remainderBuffer = Memory<byte>.Empty;

        while (true)
        {
            var inputBuffer = await stream.RefillAsync(buffer, remainderBuffer, cancellationToken);

            if (inputBuffer.IsEmpty)
            {
                yield break;
            }

            var chunkLength = _partitioner.FindChunkLength(inputBuffer.Span);
            remainderBuffer = inputBuffer.Slice(start: chunkLength);

            yield return chunkLength;
        }
    }
}