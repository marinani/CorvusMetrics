using Corvus.Application.Authentication.Abstractions;
using Corvus.Domain.Interfaces;
using Corvus.Infrastructure.Identity;
using Corvus.Infrastructure.Options;
using Corvus.Infrastructure.Persistence;
using Corvus.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Corvus.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Connection string 'DefaultConnection' was not found in configuration.");

        services.AddDbContext<CorvusDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.Configure<JwtSettings>(
            configuration.GetSection(JwtSettings.SectionName));

        services.AddScoped<IUnitOfWork>(
            serviceProvider => serviceProvider.GetRequiredService<CorvusDbContext>());

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITenantRepository, TenantRepository>();
        services.AddScoped<IAcquisitionChannelRepository, AcquisitionChannelRepository>();

        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<ITokenService, TokenService>();

        return services;
    }
}