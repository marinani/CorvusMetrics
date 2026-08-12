using Corvus.Application.Common.Results;
using Corvus.Application.Dtos;
using Corvus.Domain.Common;
using Corvus.Domain.Interfaces;
using MediatR;

namespace Corvus.Application.Queries.Users;

public sealed class GetUsersQueryHandler
    : IRequestHandler<GetUsersQuery, Result<PagedResult<UserDto>>>
{
    private readonly IUserRepository _userRepository;

    public GetUsersQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<PagedResult<UserDto>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var pageNumber = request.PageNumber <= 0 ? 1 : request.PageNumber;
        var pageSize = request.PageSize <= 0 ? 10 : request.PageSize;

        var paged = await _userRepository.GetPagedAsync(
            pageNumber,
            pageSize,
            request.FirstName,
            request.LastName,
            request.Role,
            request.Status,
            cancellationToken);

        var dtos = paged.Items
            .Select(user => new UserDto(
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email,
                user.Role.ToString(), user.IsActive))
            .ToList();

        var result = new PagedResult<UserDto>(dtos, pageNumber, pageSize, paged.TotalCount);

        return Result<PagedResult<UserDto>>.Success(result);
    }
}
