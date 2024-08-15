using System.Reflection;
using VoiceFlex.DTO;

namespace VoiceFlex.ApiEndpoints;

public static class ApiEndpoints
{
    public static WebApplication MapApiEndpoints(this WebApplication app)
    {
        app.MapGet("/api", ServiceAlive);

        app.UseHttpsRedirection()
            .UseSwagger()
            .UseSwaggerUI();

        return app;
    }

    /// <summary>
    /// Gets the service version if the service is "alive".
    /// </summary>
    private static ServiceAlive ServiceAlive()
        => new() { Version = Assembly.GetEntryAssembly().GetName().Version.ToString() };
}
