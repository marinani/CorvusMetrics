namespace Corvus.Application.Dtos;

public sealed record LoginResponse(string Token, DateTime ExpiresAtUtc, UserDto User);