namespace Corvus.Application.Dtos;

public sealed record TenantDto(
    Guid Id,
    string Name,
    string CNPJ,
    string Email,
    bool IsActive);