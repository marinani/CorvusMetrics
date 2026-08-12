using Corvus.Domain.Common;

namespace Corvus.Domain.Entities;

public sealed class Tenant : BaseEntity, IAggregateRoot
{
    private Tenant()
    {
    }

    public Tenant(string name, string cnpj, string email)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Tenant name must not be null or whitespace.", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(cnpj))
        {
            throw new ArgumentException("Tenant CNPJ must not be null or whitespace.", nameof(cnpj));
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Tenant email must not be null or whitespace.", nameof(email));
        }

        Name = name;
        CNPJ = cnpj;
        Email = email;
    }

    public string Name { get; private set; } = null!;

    public string CNPJ { get; private set; } = null!;

    public string Email { get; private set; } = null!;

    public void Update(string name, string cnpj, string email)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Tenant name must not be null or whitespace.", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(cnpj))
        {
            throw new ArgumentException("Tenant CNPJ must not be null or whitespace.", nameof(cnpj));
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Tenant email must not be null or whitespace.", nameof(email));
        }

        Name = name;
        CNPJ = cnpj;
        Email = email;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAtUtc = DateTime.UtcNow;
    }
}