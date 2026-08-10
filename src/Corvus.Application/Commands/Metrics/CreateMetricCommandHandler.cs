using Corvus.Application.Common.Results;
using Corvus.Domain.Entities;
using Corvus.Domain.Interfaces;
using MediatR;

namespace Corvus.Application.Commands.Metrics;

public sealed class CreateMetricCommandHandler
    : IRequestHandler<CreateMetricCommand, Result<Guid>>
{
    private readonly IMetricRepository _metricRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateMetricCommandHandler(IMetricRepository metricRepository, IUnitOfWork unitOfWork)
    {
        _metricRepository = metricRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateMetricCommand request, CancellationToken cancellationToken)
    {
        var metric = new Metric(
            request.Name,
            request.Value,
            request.Unit,
            request.Type,
            request.Description);

        await _metricRepository.AddAsync(metric, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(metric.Id);
    }
}