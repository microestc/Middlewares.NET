# SETUP

Example
```
using Middlewares.NET;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MsTest;

[TestClass]
public class UnitTest
{
    [DataTestMethod]
    [DataRow(new[] { "v1", "v2", "v3" })]
    [DataRow(new[] { "v1", "v2" })]
    public async Task NoServices_TestMethodAsync(string[] args)
    {
        var mw = MiddlewareBuilder.New();

        mw.Run(context =>
        {
            if (context is StringsContext ctx)
            {
                ctx.Put();
            }
            return Task.CompletedTask;
        });

        var request = mw.Build();
        var context = new StringsContext(args);

        await request(context);

        Assert.AreEqual(true, context.Yes);
    }

    [DataTestMethod]
    [DataRow(new[] { "v1", "v2", "v3" })]
    [DataRow(new[] { "v1", "v2" })]
    public async Task Services_TestMethodAsync(string[] args)
    {
        var services = new ServiceCollection();
        services.AddSingleton<TestMiddleware>();
        var provider = services.BuildServiceProvider();
        var mw = MiddlewareBuilder.New(provider);

        mw.UseMiddleware<TestMiddleware>();

        var context = new StringsContext(args);

        var request = mw.Build();

        await request(context);

        Assert.AreEqual(true, context.Yes);
    }
}

public class StringsContext : IContext
{
    private string[] _args;

    public StringsContext(params string[] args)
    {
        _args = args;
    }

    public bool Yes { get; private set; }

    public void Put()
    {
        if (_args.Length > 2) Yes = true;
    }
}

public class TestMiddleware : IMiddleware
{
    public Task InvokeAsync(IContext context, RequestDelegate next)
    {
        if (context is StringsContext ctx)
        {
            ctx.Put();
        }
        return Task.CompletedTask;
    }
}
```