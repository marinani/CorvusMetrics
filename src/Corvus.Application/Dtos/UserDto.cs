namespace Corvus.Application.Dtos;

public sealed record UserDto(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string Role,
    bool IsActive);