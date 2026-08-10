using Corvus.Application.Common.Results;
using Corvus.Domain.Interfaces;
using MediatR;

namespace Corvus.Application.Commands.Metrics;

public sealed class DeleteMetricCommandHandler : IRequestHandler<DeleteMetricCommand, Result>
{
    private readonly IMetricRepository _metricRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteMetricCommandHandler(IMetricRepository metricRepository, IUnitOfWork unitOfWork)
    {
        _metricRepository = metricRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteMetricCommand request, CancellationToken cancellationToken)
    {
        var metric = await _metricRepository.GetByIdAsync(request.Id, cancellationToken);

        if (metric is null)
        {
            return Result.Failure(
                new Error("Metric.NotFound", $"Metric with id '{request.Id}' was not found."));
        }

        _metricRepository.Remove(metric);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}