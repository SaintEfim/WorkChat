using System.Reflection;

namespace Service.Authify.API.Helpers;

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

        MapExceptions(exceptionTypes);

        services.AddSingleton(ExceptionMapping);
    }

    private static void MapExceptions(IEnumerable<Type> exceptionTypes)
    {
        foreach (var exceptionType in exceptionTypes)
        {
            var titleField = exceptionType.GetField("Title", BindingFlags.Public | BindingFlags.Instance);
            var statusCodeField = exceptionType.GetField("StatusCode", BindingFlags.Public | BindingFlags.Instance);

            if (titleField == null || statusCodeField == null) continue;
            var constructor = exceptionType.GetConstructor(Type.EmptyTypes);
            if (constructor == null) continue;
            var instance = constructor.Invoke(null);
            var title = (string?)titleField.GetValue(instance);
            var statusCode = (int?)statusCodeField.GetValue(instance) ?? 0;

            ExceptionMapping.Add(exceptionType, (title, statusCode));
        }
    }
}