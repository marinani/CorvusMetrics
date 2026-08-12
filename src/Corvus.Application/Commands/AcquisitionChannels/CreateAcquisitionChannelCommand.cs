using Corvus.Application.Common.Results;
using MediatR;

namespace Corvus.Application.Commands.AcquisitionChannels;

public sealed record CreateAcquisitionChannelCommand(
    string Name,
    string Color) : IRequest<Result<Guid>>;