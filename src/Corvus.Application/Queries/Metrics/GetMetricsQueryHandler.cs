using Corvus.Application.Common.Results;
using Corvus.Application.Dtos;
using Corvus.Domain.Interfaces;
using MediatR;

namespace Corvus.Application.Queries.Metrics;

public sealed class GetMetricsQueryHandler
    : IRequestHandler<GetMetricsQuery, Result<IReadOnlyList<MetricDto>>>
{
    private readonly IMetricRepository _metricRepository;

    public GetMetricsQueryHandler(IMetricRepository metricRepository)
    {
        _metricRepository = metricRepository;
    }

    public async Task<Result<IReadOnlyList<MetricDto>>> Handle(
        GetMetricsQuery request,
        CancellationToken cancellationToken)
    {
        var metrics = await _metricRepository.GetAllAsync(cancellationToken);

        var dtos = metrics
            .Select(metric => new MetricDto(
                metric.Id,
                metric.Name,
                metric.Value,
                metric.Unit,
                metric.Type,
                metric.Description,
                metric.RecordedAtUtc,
                metric.CreatedAtUtc,
                metric.Tags))
            .ToList();

        return Result<IReadOnlyList<MetricDto>>.Success(dtos);
    }
}