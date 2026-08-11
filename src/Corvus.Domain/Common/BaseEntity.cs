namespace Corvus.Domain.Common;

public abstract class BaseEntity
{
    public Guid Id { get; protected set; } = Guid.NewGuid();

    public DateTime CreatedAtUtc { get; protected set; } = DateTime.UtcNow;

    public DateTime? UpdatedAtUtc { get; protected set; }

    public bool IsActive { get; private set; } = true;

    private readonly List<IDomainEvent> _domainEvents = new();

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    protected void RemoveDomainEvent(IDomainEvent domainEvent) => _domainEvents.Remove(domainEvent);

    protected void ClearDomainEvents() => _domainEvents.Clear();
}