using VoiceFlex.BLL;
using VoiceFlex.DTO;

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
    /// <remarks>
    /// Sample request:
    /// 
    ///     {
    ///         "number": "12345678901"
    ///     }
    /// </remarks>
    private static async Task<IResult> CreatePhoneNumberAsync(
        PhoneNumberDto phoneNumber, IPhoneNumberManager phoneNumberManager, IErrorManager errorManager)
        => errorManager.ErrorOrOk(await phoneNumberManager.CreatePhoneNumberAsync(phoneNumber));

    /// <summary>
    /// Assign a phone number to an account.
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// 
    ///     {
    ///         "accountId": "GUID"
    ///     }
    /// </remarks>
    /// <param name="id">Phone number id</param>
    private static async Task<IResult> UpdatePhoneNumberAsync(
        Guid id, PhoneNumberUpdateDto phoneNumberUpdate, IPhoneNumberManager phoneNumberManager, IErrorManager errorManager)
        => errorManager.ErrorOrOk(await phoneNumberManager.AssignUnassignPhoneNumberAsync(id, phoneNumberUpdate));

    /// <summary>
    /// Delete a phone number.
    /// </summary>
    /// <param name="id">Phone number id</param>
    private static async Task<IResult> DeletePhoneNumberAsync(
        Guid id, IPhoneNumberManager phoneNumberManager, IErrorManager errorManager)
        => errorManager.ErrorOrOk(await phoneNumberManager.DeletePhoneNumberAsync(id));
}
