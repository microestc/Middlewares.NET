using System.Threading.Tasks;

namespace Middleware
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
