using Corvus.Domain.Enums;
using Corvus.Domain.ValueObjects;

namespace Corvus.Application.Dtos;

public sealed record MetricDto(
    Guid Id,
    string Name,
    double Value,
    string Unit,
    MetricType Type,
    string? Description,
    DateTime RecordedAtUtc,
    DateTime CreatedAtUtc,
    IReadOnlyCollection<MetricTag> Tags);