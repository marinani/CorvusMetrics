using Corvus.Domain.Entities;
using Corvus.Domain.Enums;

namespace Corvus.Domain.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<User> Items, int TotalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        string? firstName,
        string? lastName,
        UserRole? role,
        UserStatusFilter status,
        CancellationToken cancellationToken = default);
}
