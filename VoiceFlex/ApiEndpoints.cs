using System.Reflection;
using VoiceFlex.BLL;
using VoiceFlex.DTO;

namespace VoiceFlex;

public static class ApiEndpoints
{
    public static WebApplication MapApiEndpoints(this WebApplication app)
    {
        app.MapGet("/api", ServiceAlive);
        app.MapGet("/api/phonenumbers", ListPhoneNumbersAsync);

        app.UseHttpsRedirection();
        return app;
    }

    private static ServiceAlive ServiceAlive()
        => new() { Version = Assembly.GetEntryAssembly().GetName().Version.ToString() };

    private static async Task<List<PhoneNumberDto>> ListPhoneNumbersAsync(IPhoneNumberManager phoneNumberManager)
        => await phoneNumberManager.ListPhoneNumbersAsync();
}
