using VoiceFlex.BLL;
using VoiceFlex.Data;
using VoiceFlex.DTO;
using VoiceFlex.Helpers;

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

    /// <summary>
    /// Create a new phone number.
    /// </summary>
    private static async Task<IResult> CreatePhoneNumberAsync(
        PhoneNumberDto phoneNumber, IPhoneNumberManager phoneNumberManager, IErrorManager errorManager)
        => (phoneNumber.Number is null
            || phoneNumber.Number.Length < 1
            || phoneNumber.Number.Length > 11)
            ? errorManager.Error(ErrorCodes.VOICEFLEX_0002)
            : Results.Ok(await phoneNumberManager.CreatePhoneNumberAsync(phoneNumber));

    /// <summary>
    /// Assign a phone number to an account.
    /// </summary>
    private static async Task<IResult> UpdatePhoneNumberAsync(
        Guid id, PhoneNumberUpdateDto phoneNumberUpdate, IPhoneNumberManager phoneNumberManager, IErrorManager errorManager)
    {
        var callResult = await phoneNumberManager.UpdatePhoneNumberAsync(id, phoneNumberUpdate);
        return (callResult is CallError error)
            ? errorManager.Error(error.Code)
            : Results.Ok(callResult);
    }

    /// <summary>
    /// Delete a phone number.
    /// </summary>
    private static async Task DeletePhoneNumberAsync(
        Guid id, IPhoneNumberManager phoneNumberManager)
        => await phoneNumberManager.DeletePhoneNumberAsync(id);
}
