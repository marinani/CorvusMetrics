using Corvus.Application.Common.Results;
using MediatR;

namespace Corvus.Application.Commands.Tenants;

public sealed record UpdateTenantCommand(
    Guid Id,
    string Name,
    string CNPJ,
    string Email) : IRequest<Result>;