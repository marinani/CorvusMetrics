using Corvus.Application.Common.Results;
using MediatR;

namespace Corvus.Application.Commands.AcquisitionChannels;

public sealed record UpdateAcquisitionChannelCommand(
    Guid Id,
    string Name,
    string Color) : IRequest<Result>;