using Corvus.Application.Common.Results;
using Corvus.Application.Dtos;
using MediatR;

namespace Corvus.Application.Queries.Metrics;

public sealed record GetMetricsQuery() : IRequest<Result<IReadOnlyList<MetricDto>>>;