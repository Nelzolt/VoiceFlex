using VoiceFlex.BLL;
using VoiceFlex.DTO;
using VoiceFlex.Models;

namespace VoiceFlex;

public static class ApiEndpoints
{
    public static WebApplication MapApiEndpoints(this WebApplication app)
    {
        app.MapGet("/api", ServiceAlive);
        app.MapGet("/api/phonenumbers", ListPhoneNumbersAsync);

        return app;
    }

    private static ServiceAlive ServiceAlive()
        => new() { Version = "1.0.0" };

    private static async Task<List<PhoneNumber>> ListPhoneNumbersAsync(IPhoneNumberManager phoneNumberManager)
        => await phoneNumberManager.ListPhoneNumbersAsync();
}
