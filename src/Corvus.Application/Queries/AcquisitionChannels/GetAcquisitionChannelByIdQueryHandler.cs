using Corvus.Application.Common.Results;
using Corvus.Application.Dtos;
using Corvus.Domain.Interfaces;
using MediatR;

namespace Corvus.Application.Queries.AcquisitionChannels;

public sealed class GetAcquisitionChannelByIdQueryHandler
    : IRequestHandler<GetAcquisitionChannelByIdQuery, Result<AcquisitionChannelDto>>
{
    private readonly IAcquisitionChannelRepository _acquisitionChannelRepository;

    public GetAcquisitionChannelByIdQueryHandler(IAcquisitionChannelRepository acquisitionChannelRepository)
    {
        _acquisitionChannelRepository = acquisitionChannelRepository;
    }

    public async Task<Result<AcquisitionChannelDto>> Handle(
        GetAcquisitionChannelByIdQuery request,
        CancellationToken cancellationToken)
    {
        var channel = await _acquisitionChannelRepository.GetByIdAsync(request.Id, cancellationToken);

        if (channel is null)
        {
            return Result<AcquisitionChannelDto>.Failure(
                new Error("AcquisitionChannel.NotFound", $"Acquisition channel with id '{request.Id}' was not found."));
        }

        var dto = new AcquisitionChannelDto(
            channel.Id,
            channel.Name,
            channel.Color,
            channel.IsActive,
            channel.CreatedAtUtc,
            channel.UpdatedAtUtc);

        return Result<AcquisitionChannelDto>.Success(dto);
    }
}