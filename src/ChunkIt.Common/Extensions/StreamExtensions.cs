namespace ChunkIt.Common.Extensions;

public static class StreamExtensions
{
    public static async ValueTask<Memory<byte>> RefillAsync(
        this Stream stream,
        Memory<byte> buffer,
        Memory<byte> remainderBuffer,
        CancellationToken cancellationToken = default
    )
    {
        Memory<byte> readBuffer;

        if (remainderBuffer.IsEmpty)
        {
            readBuffer = buffer;
        }
        else
        {
            remainderBuffer.CopyTo(buffer);
            readBuffer = buffer.Slice(start: remainderBuffer.Length);
        }

        var readLength = await stream.ReadAsync(readBuffer, cancellationToken);

        return buffer.Slice(
            start: 0,
            length: remainderBuffer.Length + readLength
        );
    }
}