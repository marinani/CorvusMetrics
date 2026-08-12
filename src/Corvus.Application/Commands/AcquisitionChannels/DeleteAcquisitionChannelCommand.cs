using Corvus.Application.Common.Results;
using MediatR;

namespace Corvus.Application.Commands.AcquisitionChannels;

public sealed record DeleteAcquisitionChannelCommand(Guid Id) : IRequest<Result>;