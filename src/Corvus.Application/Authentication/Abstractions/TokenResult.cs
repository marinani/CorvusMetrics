namespace Corvus.Application.Authentication.Abstractions;

public sealed record TokenResult(string Token, DateTime ExpiresAtUtc);