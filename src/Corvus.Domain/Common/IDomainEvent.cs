namespace Corvus.Domain.Common;

public interface IDomainEvent
{
    Guid AggregateId { get; }
}