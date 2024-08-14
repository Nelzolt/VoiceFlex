using VoiceFlex.BLL;
using VoiceFlex.DTO;
using VoiceFlex.Models;

namespace VoiceFlex.ApiEndpoints;

public static class PhoneNumberApiEndpoints
{
    public static WebApplication MapPhoneNumberApiEndpoints(this WebApplication app)
    {
        app.MapPost("/api/phonenumbers", CreatePhoneNumberAsync);
        app.MapPatch("/api/phonenumbers/{id:guid}", UpdatePhoneNumberAsync);
        app.MapDelete("/api/phonenumbers/{id:guid}", DeletePhoneNumberAsync);

        return app;
    }

    private static async Task<PhoneNumberDto> CreatePhoneNumberAsync(PhoneNumberDto phoneNumber, IPhoneNumberManager phoneNumberManager)
        => await phoneNumberManager.CreatePhoneNumberAsync(phoneNumber);

    private static async Task<PhoneNumber> UpdatePhoneNumberAsync(Guid id, PhoneNumberUpdateDto phoneNumberUpdate, IPhoneNumberManager phoneNumberManager)
        => await phoneNumberManager.UpdatePhoneNumberAsync(id, phoneNumberUpdate);

    private static async Task DeletePhoneNumberAsync(Guid id, IPhoneNumberManager phoneNumberManager)
        => await phoneNumberManager.DeletePhoneNumberAsync(id);
}
