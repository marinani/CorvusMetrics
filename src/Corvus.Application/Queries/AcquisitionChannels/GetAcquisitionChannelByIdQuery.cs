using Corvus.Application.Common.Results;
using Corvus.Application.Dtos;
using MediatR;

namespace Corvus.Application.Queries.AcquisitionChannels;

public sealed record GetAcquisitionChannelByIdQuery(Guid Id) : IRequest<Result<AcquisitionChannelDto>>;