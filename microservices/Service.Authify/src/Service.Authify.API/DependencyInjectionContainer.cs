using Service.Authify.Data.Helpers;
using Service.Authify.Data.PostgreSql.Repository;
using Service.Authify.Data.Repository;
using Service.Authify.Domain.Services;

namespace Service.Authify.API;

public static class DependencyInjectionContainer
{
    public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
    {
        services.AddScoped<GenerateClaimsHelper>();
        services.AddScoped<GenerateKeyHelper>();
        services.AddScoped<GenerateTokenHelper>();
        services.AddScoped<IUserCredentialRepository, UserCredentialRepository>();
        services.AddScoped<IUserCredentialManager, UserCredentialManager>();
        services.AddScoped<IUserCredentialProvider, UserCredentialProvider>();

        return services;
    }
}