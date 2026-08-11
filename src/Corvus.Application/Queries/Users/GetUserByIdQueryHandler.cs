using Corvus.Application.Common.Results;
using Corvus.Application.Dtos;
using Corvus.Domain.Interfaces;
using MediatR;

namespace Corvus.Application.Queries.Users;

public sealed class GetUserByIdQueryHandler
    : IRequestHandler<GetUserByIdQuery, Result<UserDto>>
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);

        if (user is null)
        {
            return Result<UserDto>.Failure(
                new Error("User.NotFound", $"User with id '{request.Id}' was not found."));
        }

        var dto = new UserDto(
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email,
            user.Role.ToString());

        return Result<UserDto>.Success(dto);
    }
}