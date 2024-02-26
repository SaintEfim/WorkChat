using System.Reflection;
using AutoMapper;

namespace Service.Authify.API.Helpers;

public static class AutoMapperExtensions
{
    public static void AddAutoMapperFromAllAssemblies(this IServiceCollection services)
    {
        var assemblies = Assembly.GetExecutingAssembly().GetReferencedAssemblies()
            .Select(Assembly.Load)
            .Union(new[] { Assembly.GetExecutingAssembly() });

        var profileTypes = assemblies
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => typeof(Profile).IsAssignableFrom(type) && !type.IsAbstract);

        services.AddAutoMapper(profileTypes.ToArray());
    }
}