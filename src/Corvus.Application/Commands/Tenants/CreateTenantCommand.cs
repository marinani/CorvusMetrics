using Corvus.Application.Common.Results;
using MediatR;

namespace Corvus.Application.Commands.Tenants;

public sealed record CreateTenantCommand(
    string Name,
    string CNPJ,
    string Email) : IRequest<Result<Guid>>;