using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Middleware
{
    public interface IMiddlewareBuilder
    {
        IMiddlewareBuilder Use(Func<RequestDelegate, RequestDelegate> middleware);

        RequestDelegate Build();

        IServiceProvider ServiceProvider { get; }
    }

    public class MiddlewareBuilder : IMiddlewareBuilder
    {
        private readonly List<Func<RequestDelegate, RequestDelegate>> _middlewares = new List<Func<RequestDelegate, RequestDelegate>>();
        public RequestDelegate Build()
        {
            RequestDelegate @delegate = context => Task.CompletedTask;
            for (var index = _middlewares.Count - 1; index >= 0; index--)
            {
                @delegate = _middlewares[index](@delegate);
            }
            return @delegate;
        }

        public IMiddlewareBuilder Use(Func<RequestDelegate, RequestDelegate> middleware)
        {
            _middlewares.Add(middleware);
            return this;
        }

        public IServiceProvider ServiceProvider { get; }

        public MiddlewareBuilder() { }

        public MiddlewareBuilder(IServiceProvider serviceProvider) => ServiceProvider = serviceProvider;

        public static IMiddlewareBuilder New() => new MiddlewareBuilder();

        public static IMiddlewareBuilder New(IServiceProvider serviceProvider) => new MiddlewareBuilder(serviceProvider);
    }
}
