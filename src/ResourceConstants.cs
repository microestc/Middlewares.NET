using System;

namespace Middleware;

public static class ResourceConstants
{
    public static string MiddlewareNotJoinServices(Type type)
    {
        return $"{type.Name} middleware doesn't join the services.";
    }

    public static string NotImplicitRefMiddleware(Type type)
    {
        return $"There is no implicit reference conversion from '{type.Name}' to 'IMiddleware'.";
    }
}
