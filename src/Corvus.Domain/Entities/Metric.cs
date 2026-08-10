using Corvus.Domain.Common;
using Corvus.Domain.Enums;
using Corvus.Domain.ValueObjects;

namespace Corvus.Domain.Entities;

public sealed class Metric : BaseEntity, IAggregateRoot
{
    private readonly List<MetricTag> _tags = new();

    private Metric()
    {
    }

    public Metric(string name, double value, string unit, MetricType type, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Metric name must not be null or whitespace.", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(unit))
        {
            throw new ArgumentException("Metric unit must not be null or whitespace.", nameof(unit));
        }

        Name = name;
        Value = value;
        Unit = unit;
        Type = type;
        Description = description;
        RecordedAtUtc = DateTime.UtcNow;
    }

    public string Name { get; private set; } = null!;

    public double Value { get; private set; }

    public string Unit { get; private set; } = null!;

    public MetricType Type { get; private set; }

    public string? Description { get; private set; }

    public DateTime RecordedAtUtc { get; private set; }

    public IReadOnlyCollection<MetricTag> Tags => _tags.AsReadOnly();

    public void Update(string name, double value, string unit, MetricType type, string? description)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Metric name must not be null or whitespace.", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(unit))
        {
            throw new ArgumentException("Metric unit must not be null or whitespace.", nameof(unit));
        }

        Name = name;
        Value = value;
        Unit = unit;
        Type = type;
        Description = description;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void AddTag(string key, string value) => _tags.Add(new MetricTag(key, value));

    public void ClearTags() => _tags.Clear();
}