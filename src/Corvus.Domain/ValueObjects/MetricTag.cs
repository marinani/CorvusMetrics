using Corvus.Domain.Common;

namespace Corvus.Domain.ValueObjects;

public sealed class MetricTag : ValueObject
{
    public MetricTag(string key, string value)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("Tag key must not be null or whitespace.", nameof(key));
        }

        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Tag value must not be null or whitespace.", nameof(value));
        }

        Key = key;
        Value = value;
    }

    private MetricTag()
    {
    }

    public string Key { get; } = null!;

    public string Value { get; } = null!;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Key;
        yield return Value;
    }
}