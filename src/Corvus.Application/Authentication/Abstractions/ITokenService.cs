using Corvus.Domain.Entities;

namespace Corvus.Application.Authentication.Abstractions;

public interface ITokenService
{
    TokenResult CreateToken(User user);
}