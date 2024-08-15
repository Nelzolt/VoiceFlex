using VoiceFlex.BLL;
using VoiceFlex.DTO;
using VoiceFlex.Helpers;

namespace VoiceFlex.ApiEndpoints;

public static class AccountApiEndpoints
{
    public static WebApplication MapAccountApiEndpoints(this WebApplication app)
    {
        app.MapPost("/api/accounts", CreateAccountAsync);
        app.MapGet("/api/accounts/{id:guid}/phonenumbers", GetAccountWithPhoneNumbersAsync);
        app.MapPatch("/api/accounts/{id:guid}", UpdateAccountAsync);

        return app;
    }

    /// <summary>
    /// Create a new account.
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// 
    ///     {
    ///         "description": "John Mary Doe",
    ///         "status": 1
    ///     }
    /// </remarks>
    private static async Task<AccountDto> CreateAccountAsync(AccountDto account, IAccountManager accountManager)
        => await accountManager.CreateAccountAsync(account);

    /// <summary>
    /// Get all phone numbers for an account.
    /// </summary>
    /// <param name="id">Account id</param>
    private static async Task<IResult> GetAccountWithPhoneNumbersAsync(
        Guid id, IAccountManager accountManager, IErrorManager errorManager)
    {
        var accountDto = await accountManager.GetAccountWithPhoneNumbersAsync(id);
        return accountDto is null
            ? errorManager.Error(ErrorCodes.VOICEFLEX_0001)
            : Results.Ok(accountDto);
    }

    /// <summary>
    /// Set an account to active (1) or suspended (0).
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// 
    ///     {
    ///         "status": 0
    ///     }
    /// </remarks>
    /// <param name="id">Account id</param>
    private static async Task<IResult> UpdateAccountAsync(
        Guid id, AccountUpdateDto accountUpdate, IAccountManager accountManager, IErrorManager errorManager)
    {
        var account = await accountManager.UpdateAccountAsync(id, accountUpdate);
        return account is null
            ? errorManager.Error(ErrorCodes.VOICEFLEX_0001)
            : Results.Ok(account);
    }
}
