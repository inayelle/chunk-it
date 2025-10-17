using System.Security.Cryptography;
using ChunkIt.Common;
using ChunkIt.Common.Abstractions;
using ChunkIt.Common.Extensions;

namespace ChunkIt.Metrics.Deduplication.Controllers;

internal sealed class AmendSourceFilesController : IController
{
    private const int Kilobyte = 1024;
    private const int Megabyte = 1024 * Kilobyte;

    private const int BufferSize = Megabyte / 2;

    public async Task Run()
    {
        foreach (var sourceFile in SourceFiles.Values)
        {
            await AmendSourceFile(sourceFile);
        }
    }

    private static async Task AmendSourceFile(SourceFile sourceFile)
    {
        await using var sourceFileStream = sourceFile.OpenFileStream(BufferSize);
        await using var destinationFileStream = new FileStream(
            sourceFile.GenerateVersionedPath("0"),
            FileMode.Create,
            FileAccess.Write,
            FileShare.None,
            BufferSize
        );

        using var readBufferLease = new ArrayPoolLease<byte>(BufferSize);
        var readBuffer = readBufferLease.Memory;

        using var randomBufferLease = new ArrayPoolLease<byte>(32);
        var randomBuffer = randomBufferLease.Memory;

        while (true)
        {
            var inputBuffer = await sourceFileStream.RefillAsync(readBuffer, Memory<byte>.Empty);

            if (inputBuffer.Length < BufferSize)
            {
                await destinationFileStream.WriteAsync(inputBuffer);

                break;
            }

            var offset = CalculateRandomOffset(inputBuffer.Length);
            var (headBuffer, remainderBuffer) = inputBuffer.Split(offset);

            RandomNumberGenerator.Fill(randomBuffer.Span);

            await destinationFileStream.WriteAsync(headBuffer);
            await destinationFileStream.WriteAsync(randomBuffer);
            await destinationFileStream.WriteAsync(remainderBuffer);
        }

        await destinationFileStream.FlushAsync();
    }

    private static int CalculateRandomOffset(int bufferLength)
    {
        var minOffset = Math.Max(BufferSize - 2 * Kilobyte, 0);
        var maxOffset = Math.Min(BufferSize + 2 * Kilobyte, bufferLength);

        return Random.Shared.Next(minOffset, maxOffset);
    }
}