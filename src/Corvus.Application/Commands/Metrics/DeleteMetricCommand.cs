using Corvus.Application.Common.Results;
using MediatR;

namespace Corvus.Application.Commands.Metrics;

public sealed record DeleteMetricCommand(Guid Id) : IRequest<Result>;