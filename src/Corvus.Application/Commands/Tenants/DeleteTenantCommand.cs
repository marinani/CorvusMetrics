using Corvus.Application.Common.Results;
using MediatR;

namespace Corvus.Application.Commands.Tenants;

public sealed record DeleteTenantCommand(Guid Id) : IRequest<Result>;