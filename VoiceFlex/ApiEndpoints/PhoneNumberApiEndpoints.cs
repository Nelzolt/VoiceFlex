using VoiceFlex.BLL;
using VoiceFlex.DTO;

namespace VoiceFlex.ApiEndpoints;

public static class PhoneNumberApiEndpoints
{
    public static WebApplication MapPhoneNumberApiEndpoints(this WebApplication app)
    {
        app.MapPost("/api/phonenumbers", CreatePhoneNumberAsync);

        return app;
    }

    private static async Task<PhoneNumberDto> CreatePhoneNumberAsync(PhoneNumberDto phoneNumber, IPhoneNumberManager phoneNumberManager)
        => await phoneNumberManager.CreatePhoneNumberAsync(phoneNumber);
}
