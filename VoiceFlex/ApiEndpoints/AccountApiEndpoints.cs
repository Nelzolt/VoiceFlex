﻿using VoiceFlex.BLL;
using VoiceFlex.DTO;
using VoiceFlex.Helpers;
using VoiceFlex.Models;

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

    private static async Task<AccountDto> CreateAccountAsync(AccountDto account, IAccountManager accountManager)
        => await accountManager.CreateAccountAsync(account);

    private static async Task<IResult> GetAccountWithPhoneNumbersAsync(Guid id, IAccountManager accountManager, IErrorManager errorManager)
    {
        var accountDto = await accountManager.GetAccountWithPhoneNumbersAsync(id);
        return accountDto is null
            ? errorManager.Error(ErrorCodes.VOICEFLEX_0001)
            : Results.Ok(accountDto);
    }

    private static async Task<Account> UpdateAccountAsync(Guid id, AccountUpdateDto accountUpdate, IAccountManager accountManager)
        => await accountManager.UpdateAccountAsync(id, accountUpdate);
}
