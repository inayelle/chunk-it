using System.Text;

namespace ChunkIt.Abstractions;

public sealed class DescriptionBuilder
{
    private readonly string _name;
    private readonly Dictionary<string, object> _params;

    public DescriptionBuilder(string name)
    {
        _name = name;
        _params = new Dictionary<string, object>();
    }

    public DescriptionBuilder AddParameter(string name, object value)
    {
        _params.Add(name, value);

        return this;
    }

    public string Build()
    {
        if (_params.Count == 0)
        {
            return _name;
        }

        var sb = new StringBuilder();

        sb.Append($"{_name} (");

        var paramsText = _params.Select(pair => $"{pair.Key}: {pair.Value}");

        sb.AppendJoin(", ", paramsText);

        sb.Append(')');

        return sb.ToString();
    }
}