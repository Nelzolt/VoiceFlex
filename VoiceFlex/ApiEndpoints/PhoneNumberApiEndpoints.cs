using VoiceFlex.BLL;
using VoiceFlex.DTO;

namespace VoiceFlex.ApiEndpoints;

public static class PhoneNumberApiEndpoints
{
    public static WebApplication MapPhoneNumberApiEndpoints(this WebApplication app)
    {
        app.MapGet("/api/phonenumbers", ListPhoneNumbersAsync);

        return app;
    }

    private static async Task<List<PhoneNumberDto>> ListPhoneNumbersAsync(IPhoneNumberManager phoneNumberManager)
        => await phoneNumberManager.ListPhoneNumbersAsync();
}
