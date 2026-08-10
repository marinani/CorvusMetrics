using Microsoft.OpenApi;

namespace Corvus.Api.Extensions;

public static class SwaggerExtensions
{
    private const string SecuritySchemeId = "bearer";

    public static IServiceCollection AddSwaggerWithJwt(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(
                "v1",
                new OpenApiInfo
                {
                    Title = "Corvus API",
                    Version = "v1",
                    Description = "REST API for the Corvus metrics platform built with DDD and CQRS."
                });

            options.AddSecurityDefinition(
                SecuritySchemeId,
                new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter only the JWT token without the 'Bearer ' prefix."
                });

            options.AddSecurityRequirement(
                document => new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference(SecuritySchemeId, document)] = []
                });
        });

        return services;
    }
}