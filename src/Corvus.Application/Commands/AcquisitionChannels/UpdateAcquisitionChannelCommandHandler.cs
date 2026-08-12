using Corvus.Application.Common.Results;
using Corvus.Domain.Interfaces;
using MediatR;

namespace Corvus.Application.Commands.AcquisitionChannels;

public sealed class UpdateAcquisitionChannelCommandHandler
    : IRequestHandler<UpdateAcquisitionChannelCommand, Result>
{
    private readonly IAcquisitionChannelRepository _acquisitionChannelRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateAcquisitionChannelCommandHandler(
        IAcquisitionChannelRepository acquisitionChannelRepository,
        IUnitOfWork unitOfWork)
    {
        _acquisitionChannelRepository = acquisitionChannelRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateAcquisitionChannelCommand request, CancellationToken cancellationToken)
    {
        var channel = await _acquisitionChannelRepository.GetByIdAsync(request.Id, cancellationToken);

        if (channel is null)
        {
            return Result.Failure(
                new Error("AcquisitionChannel.NotFound", $"Acquisition channel with id '{request.Id}' was not found."));
        }

        channel.Update(request.Name, request.Color);

        _acquisitionChannelRepository.Update(channel);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}