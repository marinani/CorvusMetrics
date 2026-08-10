using Corvus.Application.Common.Results;
using Corvus.Application.Dtos;
using Corvus.Domain.Interfaces;
using MediatR;

namespace Corvus.Application.Queries.Metrics;

public sealed class GetMetricByIdQueryHandler
    : IRequestHandler<GetMetricByIdQuery, Result<MetricDto>>
{
    private readonly IMetricRepository _metricRepository;

    public GetMetricByIdQueryHandler(IMetricRepository metricRepository)
    {
        _metricRepository = metricRepository;
    }

    public async Task<Result<MetricDto>> Handle(GetMetricByIdQuery request, CancellationToken cancellationToken)
    {
        var metric = await _metricRepository.GetByIdAsync(request.Id, cancellationToken);

        if (metric is null)
        {
            return Result<MetricDto>.Failure(
                new Error("Metric.NotFound", $"Metric with id '{request.Id}' was not found."));
        }

        var dto = new MetricDto(
            metric.Id,
            metric.Name,
            metric.Value,
            metric.Unit,
            metric.Type,
            metric.Description,
            metric.RecordedAtUtc,
            metric.CreatedAtUtc,
            metric.Tags);

        return Result<MetricDto>.Success(dto);
    }
}