using Corvus.Domain.Entities;
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

    public async Task AddAsync(User entity, CancellationToken cancellationToken = default)
        => await _dbContext.Users.AddAsync(entity, cancellationToken);

    public void Update(User entity) => _dbContext.Users.Update(entity);

    public void Remove(User entity) => _dbContext.Users.Remove(entity);
}