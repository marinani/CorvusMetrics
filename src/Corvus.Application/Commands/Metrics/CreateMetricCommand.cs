using Corvus.Application.Common.Results;
using Corvus.Domain.Enums;
using MediatR;

namespace Corvus.Application.Commands.Metrics;

public sealed record CreateMetricCommand(
    string Name,
    double Value,
    string Unit,
    MetricType Type,
    string? Description) : IRequest<Result<Guid>>;