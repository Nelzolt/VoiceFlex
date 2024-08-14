using VoiceFlex.BLL;
using VoiceFlex.DTO;

namespace VoiceFlex.ApiEndpoints;

public static class AccountApiEndpoints
{
    public static WebApplication MapAccountApiEndpoints(this WebApplication app)
    {
        app.MapPost("/api/accounts", CreateAccountAsync);
        app.MapGet("/api/accounts/{id:guid}/phonenumbers", GetAccountWithPhoneNumbersAsync);

        return app;
    }

    private static async Task<AccountDto> CreateAccountAsync(AccountDto account, IAccountManager accountManager)
        => await accountManager.CreateAccountAsync(account);

    private static async Task<AccountDto> GetAccountWithPhoneNumbersAsync(Guid id, IAccountManager accountManager)
        => await accountManager.GetAccountWithPhoneNumbersAsync(id);
}
