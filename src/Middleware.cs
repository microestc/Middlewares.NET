using System.Threading.Tasks;

namespace Middlewares.NET
{
    public interface IMiddleware
    {
        Task InvokeAsync(IContext context, RequestDelegate next);
    }

    public abstract class Middleware : IMiddleware
    {
        public abstract Task InvokeAsync(IContext context, RequestDelegate next);
    }
}
