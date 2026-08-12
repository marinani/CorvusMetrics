using Corvus.Application.Common.Results;
using Corvus.Domain.Entities;
using Corvus.Domain.Interfaces;
using MediatR;

namespace Corvus.Application.Commands.AcquisitionChannels;

public sealed class CreateAcquisitionChannelCommandHandler
    : IRequestHandler<CreateAcquisitionChannelCommand, Result<Guid>>
{
    private readonly IAcquisitionChannelRepository _acquisitionChannelRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAcquisitionChannelCommandHandler(
        IAcquisitionChannelRepository acquisitionChannelRepository,
        IUnitOfWork unitOfWork)
    {
        _acquisitionChannelRepository = acquisitionChannelRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateAcquisitionChannelCommand request, CancellationToken cancellationToken)
    {
        if (await _acquisitionChannelRepository.ExistsByNameAsync(request.Name, cancellationToken))
        {
            return Result<Guid>.Failure(
                new Error("AcquisitionChannel.NameAlreadyExists", $"Name '{request.Name}' is already registered."));
        }

        var channel = new AcquisitionChannel(request.Name, request.Color);

        await _acquisitionChannelRepository.AddAsync(channel, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(channel.Id);
    }
}