using Corvus.Application.Authentication.Abstractions;
using Corvus.Application.Common.Results;
using Corvus.Application.Dtos;
using Corvus.Domain.Interfaces;
using MediatR;

namespace Corvus.Application.Authentication.Commands;

public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public LoginCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        ITokenService tokenService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (user is null || !user.IsActive || !_passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            return Result<LoginResponse>.Failure(
                new Error("Authentication.InvalidCredentials", "Invalid email or password."));
        }

        var token = _tokenService.CreateToken(user);

        var response = new LoginResponse(
            token.Token,
            token.ExpiresAtUtc,
            new UserDto(user.Id, user.FirstName, user.LastName, user.Email, user.Role.ToString(), user.IsActive));

        return Result<LoginResponse>.Success(response);
    }
}