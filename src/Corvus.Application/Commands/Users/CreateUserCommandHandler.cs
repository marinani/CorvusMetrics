using Corvus.Application.Authentication.Abstractions;
using Corvus.Application.Common.Results;
using Corvus.Domain.Entities;
using Corvus.Domain.Interfaces;
using MediatR;

namespace Corvus.Application.Commands.Users;

public sealed class CreateUserCommandHandler
    : IRequestHandler<CreateUserCommand, Result<Guid>>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    public CreateUserCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (await _userRepository.ExistsByEmailAsync(request.Email, cancellationToken))
        {
            return Result<Guid>.Failure(
                new Error("User.EmailAlreadyExists", $"Email '{request.Email}' is already registered."));
        }

        var passwordHash = _passwordHasher.Hash(request.Password);
        var user = new User(
            request.FirstName,
            request.LastName,
            request.Email,
            passwordHash,
            request.Role);

        await _userRepository.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(user.Id);
    }
}