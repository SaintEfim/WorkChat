using System.Reflection;

namespace Service.Authify.API.Extensions.Helpers;

public static class MapExceptionsHelper
{
    private static readonly Dictionary<Type, (string? title, int statusCode)> ExceptionMapping = new();
    
    public static Dictionary<Type, (string? title, int statusCode)> MapExceptions(IEnumerable<Type> exceptionTypes)
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

        return ExceptionMapping;
    }
}