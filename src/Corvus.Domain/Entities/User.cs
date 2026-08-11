using Corvus.Domain.Common;
using Corvus.Domain.Enums;

namespace Corvus.Domain.Entities;

public sealed class User : BaseEntity, IAggregateRoot
{
    private User()
    {
    }

    public User(string firstName, string lastName, string email, string passwordHash, UserRole role = UserRole.Member)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            throw new ArgumentException("First name must not be null or whitespace.", nameof(firstName));
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            throw new ArgumentException("Last name must not be null or whitespace.", nameof(lastName));
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email must not be null or whitespace.", nameof(email));
        }

        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            throw new ArgumentException("Password hash must not be null or whitespace.", nameof(passwordHash));
        }

        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
    }

    public string FirstName { get; private set; } = null!;

    public string LastName { get; private set; } = null!;

    public string Email { get; private set; } = null!;

    public string PasswordHash { get; private set; } = null!;

    public UserRole Role { get; private set; }

    public void UpdateProfile(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void Update(string firstName, string lastName, UserRole role)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            throw new ArgumentException("First name must not be null or whitespace.", nameof(firstName));
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            throw new ArgumentException("Last name must not be null or whitespace.", nameof(lastName));
        }

        FirstName = firstName;
        LastName = lastName;
        Role = role;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void ChangePassword(string passwordHash)
    {
        PasswordHash = passwordHash;
        UpdatedAtUtc = DateTime.UtcNow;
    }
}