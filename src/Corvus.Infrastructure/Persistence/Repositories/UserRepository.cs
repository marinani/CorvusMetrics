using Corvus.Domain.Entities;
using Corvus.Domain.Enums;
using Corvus.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Corvus.Infrastructure.Persistence.Repositories;

public sealed class UserRepository : IUserRepository
{
    private readonly CorvusDbContext _dbContext;

    public UserRepository(CorvusDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _dbContext.Users
            .FirstOrDefaultAsync(user => user.Id == id, cancellationToken);

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        => await _dbContext.Users
            .FirstOrDefaultAsync(user => user.Email == email, cancellationToken);

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
        => await _dbContext.Users
            .AnyAsync(user => user.Email == email, cancellationToken);

    public async Task<(IReadOnlyList<User> Items, int TotalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        string? firstName,
        string? lastName,
        UserRole? role,
        UserStatusFilter status,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Users.AsNoTracking();

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

        query = status switch
        {
            UserStatusFilter.Active => query.Where(user => user.IsActive),
            UserStatusFilter.Inactive => query.Where(user => !user.IsActive),
            _ => query
        };

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(user => user.CreatedAtUtc)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task AddAsync(User entity, CancellationToken cancellationToken = default)
        => await _dbContext.Users.AddAsync(entity, cancellationToken);

    public void Update(User entity) => _dbContext.Users.Update(entity);

    public void Remove(User entity) => _dbContext.Users.Remove(entity);
}
