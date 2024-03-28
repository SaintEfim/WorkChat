using System.Reflection;
using Service.Authify.API.Extensions.Helpers;

namespace Service.Authify.API.Extensions;

public static class ExceptionMappingExtensions
{
    private static readonly Dictionary<Type, (string? title, int statusCode)> ExceptionMapping = new();
    
    public static void AddExceptionMappingFromAllAssemblies(this IServiceCollection services)
    {
        var assemblies = Assembly.GetExecutingAssembly().GetReferencedAssemblies()
            .Select(Assembly.Load)
            .Union(new[] { Assembly.GetExecutingAssembly() });

        var exceptionTypes = assemblies
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type =>
                typeof(Exception).IsAssignableFrom(type) &&
                type is { IsAbstract: false, IsInterface: false, IsClass: true }
            );

        MapExceptionsHelper.MapExceptions(exceptionTypes);

        services.AddSingleton(provider =>
        {
            var mapping = ExceptionMapping;
            return mapping;
        });
    }
}