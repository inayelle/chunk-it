using System.Text.Json;

namespace ChunkIt.Metrics.Host.Serialization;

internal static class ReportSerializer
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions
    {
        WriteIndented = true,
        IndentSize = 4,
        Converters =
        {
            new FloatingPointConverter<float>(),
            new FloatingPointConverter<double>(),
            new FloatingPointConverter<decimal>(),
        },
    };

    public static string Serialize<T>(T instance)
    {
        return JsonSerializer.Serialize(instance, JsonSerializerOptions);
    }
}