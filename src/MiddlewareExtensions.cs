using System;
using System.Threading.Tasks;

namespace Middleware
{

    public delegate Task RequestDelegate(IContext context);

    public static class MiddlewareExtensions
    {
        public static IMiddlewareBuilder Use(this IMiddlewareBuilder builder, Func<IContext, RequestDelegate, Task> function)
        {
            Func<RequestDelegate, RequestDelegate> @middleware = request => (context => function(context, request));
            return builder.Use(@middleware);
        }

        public static IMiddlewareBuilder Use(this IMiddlewareBuilder builder, Func<IContext, Func<Task>, Task> function)
        {
            Func<RequestDelegate, RequestDelegate> @middleware = request => (context => function(context, () => request(context)));
            return builder.Use(@middleware);
        }

        public static void Run(this IMiddlewareBuilder builder, RequestDelegate @request)
        {
            builder.Use(_ => @request);
        }

        private static IMiddlewareBuilder UseMiddlewareInterface(IMiddlewareBuilder builder, Type middlewareType)
        {
            var middleware = builder.ServiceProvider?.GetService(middlewareType) as IMiddleware;
            if (middleware == null) throw new InvalidOperationException(ResourceConstants.MiddlewareNotJoinServices(middlewareType));

            return builder.Use(next =>
            {
                return async context =>
                {
                    await middleware.InvokeAsync(context, next);
                };
            });
        }

        public static IMiddlewareBuilder UseMiddleware<TMiddleware>(this IMiddlewareBuilder builder) where TMiddleware : IMiddleware
        {
            return builder.UseMiddleware(typeof(TMiddleware));
        }

        public static IMiddlewareBuilder UseMiddleware(this IMiddlewareBuilder builder, Type middleware)
        {
            if (typeof(IMiddleware).IsAssignableFrom(middleware))
            {
                return UseMiddlewareInterface(builder, middleware);
            }
            throw new InvalidOperationException();
        }
    }
}