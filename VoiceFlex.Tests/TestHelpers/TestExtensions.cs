using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace VoiceFlex.Tests.TestHelpers;

public static class TestExtensions
{
    public static IWebHostBuilder AddSingleton<T>(this IWebHostBuilder webHostBuilder, T singleton) where T : class
        => webHostBuilder.ConfigureServices(services =>
        {
            services.AddSingleton(singleton);
        });
}
