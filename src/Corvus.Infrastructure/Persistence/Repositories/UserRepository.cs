using Corvus.Domain.Common;
using Corvus.Domain.Entities;
using Corvus.Domain.Enums;
using Corvus.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Corvus.Infrastructure.Persistence.Repositories;

public sealed class UserRepository : RepositoryBase<User>, IUserRepository
{
    public UserRepository(CorvusDbContext dbContext)
        : base(dbContext)
    {
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        => await DbContext.Users
            .FirstOrDefaultAsync(user => user.Email == email, cancellationToken);

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
        => await DbContext.Users
            .AnyAsync(user => user.Email == email, cancellationToken);

    public async Task<PagedResult<User>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        string? firstName,
        string? lastName,
        UserRole? role,
        EntityStatusFilter status,
        CancellationToken cancellationToken = default)
    {
        var query = DbContext.Users.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(firstName))
        {
            query = query.Where(user => EF.Functions.ILike(user.FirstName, $"%{firstName}%"));
        }

        if (!string.IsNullOrWhiteSpace(lastName))
        {
            query = query.Where(user => EF.Functions.ILike(user.LastName, $"%{lastName}%"));
        }

        if (role is not null)
        {
            query = query.Where(user => user.Role == role);
        }

        query = ApplyStatusFilter(query, status);

        return await ApplyPaginationAsync(query, pageNumber, pageSize, cancellationToken);
    }

    private static IQueryable<User> ApplyStatusFilter(IQueryable<User> query, EntityStatusFilter status)
        => status switch
        {
            EntityStatusFilter.Active => query.Where(user => user.IsActive),
            EntityStatusFilter.Inactive => query.Where(user => !user.IsActive),
            _ => query
        };
}