using System.Reflection;
using VoiceFlex.DTO;

namespace VoiceFlex.ApiEndpoints;

public static class ApiEndpoints
{
    public static WebApplication MapApiEndpoints(this WebApplication app)
    {
        app.MapGet("/", () => Results.Redirect("/swagger/index.html"));
        app.MapGet("/api", ServiceAlive);

        app.UseCors(builder =>
            {
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
                builder.AllowAnyOrigin(); //corsBuilder.WithOrigins("http://localhost:56573"); // for a specific url.
            })
            .UseHttpsRedirection()
            .UseSwagger()
            .UseSwaggerUI();

        return app;
    }

    /// <summary>
    /// Get the service version if the service is "alive".
    /// </summary>
    private static ServiceAlive ServiceAlive()
        => new() { Version = Assembly.GetEntryAssembly().GetName().Version.ToString() };
}
