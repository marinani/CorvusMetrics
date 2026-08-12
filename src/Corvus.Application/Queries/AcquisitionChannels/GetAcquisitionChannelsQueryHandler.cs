using Corvus.Application.Common.Results;
using Corvus.Application.Dtos;
using Corvus.Domain.Common;
using Corvus.Domain.Interfaces;
using MediatR;

namespace Corvus.Application.Queries.AcquisitionChannels;

public sealed class GetAcquisitionChannelsQueryHandler
    : IRequestHandler<GetAcquisitionChannelsQuery, Result<PagedResult<AcquisitionChannelDto>>>
{
    private readonly IAcquisitionChannelRepository _acquisitionChannelRepository;

    public GetAcquisitionChannelsQueryHandler(IAcquisitionChannelRepository acquisitionChannelRepository)
    {
        _acquisitionChannelRepository = acquisitionChannelRepository;
    }

    public async Task<Result<PagedResult<AcquisitionChannelDto>>> Handle(
        GetAcquisitionChannelsQuery request,
        CancellationToken cancellationToken)
    {
        var pageNumber = request.PageNumber <= 0 ? 1 : request.PageNumber;
        var pageSize = request.PageSize <= 0 ? 10 : request.PageSize;

        var paged = await _acquisitionChannelRepository.GetPagedAsync(
            pageNumber,
            pageSize,
            request.Name,
            request.Status,
            cancellationToken);

        var dtos = paged.Items
            .Select(channel => new AcquisitionChannelDto(
                channel.Id,
                channel.Name,
                channel.Color,
                channel.IsActive,
                channel.CreatedAtUtc,
                channel.UpdatedAtUtc))
            .ToList();

        var result = new PagedResult<AcquisitionChannelDto>(dtos, pageNumber, pageSize, paged.TotalCount);

        return Result<PagedResult<AcquisitionChannelDto>>.Success(result);
    }
}