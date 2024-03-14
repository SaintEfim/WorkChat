using Service.Authify.Domain.Helpers;
using Service.Authify.Data.PostgreSql.Repository;
using Service.Authify.Data.Repository;
using Service.Authify.Domain.Services;

namespace Service.Authify.API;

public static class DependencyInjectionContainer
{
    public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
    {
        services.AddScoped<IJwtHelper, JwtHelper>();
        services.AddScoped<IHashHelper, HashHelper>();
        services.AddScoped<IUserCredentialRepository, UserCredentialRepository>();
        services.AddScoped<IUserCredentialManager, UserCredentialManager>();
        services.AddScoped<IUserCredentialProvider, UserCredentialProvider>();

        return services;
    }
}