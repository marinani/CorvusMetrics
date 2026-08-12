using Corvus.Domain.Common;

namespace Corvus.Domain.Entities;

public sealed class AcquisitionChannel : BaseEntity, IAggregateRoot
{
    private AcquisitionChannel()
    {
    }

    public AcquisitionChannel(string name, string color)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name must not be null or whitespace.", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(color))
        {
            throw new ArgumentException("Color must not be null or whitespace.", nameof(color));
        }

        Name = name;
        Color = color;
    }

    public string Name { get; private set; } = null!;

    public string Color { get; private set; } = null!;

    public void Update(string name, string color)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name must not be null or whitespace.", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(color))
        {
            throw new ArgumentException("Color must not be null or whitespace.", nameof(color));
        }

        Name = name;
        Color = color;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAtUtc = DateTime.UtcNow;
    }
}