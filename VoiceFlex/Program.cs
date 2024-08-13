using VoiceFlex.DTO;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/api", () =>
{
    return new ServiceAlive { Version = "1.0.0" };
});

app.Run();
