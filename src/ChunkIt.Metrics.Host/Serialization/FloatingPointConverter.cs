using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ChunkIt.Metrics.Host.Serialization;

internal sealed class FloatingPointConverter<T> : JsonConverter<T>
    where T : IFloatingPoint<T>
{
    private readonly int _decimals;

    public FloatingPointConverter(int decimals = 2)
    {
        _decimals = decimals;

        if (!SupportedTypes.IsSupported(typeof(T)))
        {
            throw new ArgumentException(
                "The provided generic type T is not supported, must be float, double or decimal."
            );
        }
    }

    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return JsonSerializer.Deserialize<T>(ref reader, options);
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        value = T.Round(value, _decimals);

        switch (value)
        {
            case float floatValue:
                writer.WriteNumberValue(floatValue);
                break;
            case double doubleValue:
                writer.WriteNumberValue(doubleValue);
                break;
            case decimal decimalValue:
                writer.WriteNumberValue(decimalValue);
                break;
        }
    }
}

file static class SupportedTypes
{
    private static readonly IReadOnlySet<Type> Types = new HashSet<Type>(capacity: 3)
    {
        typeof(float),
        typeof(double),
        typeof(decimal),
    };

    public static bool IsSupported(Type type)
    {
        return Types.Contains(type);
    }
}