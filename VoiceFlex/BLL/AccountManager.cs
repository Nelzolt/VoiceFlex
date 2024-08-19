using VoiceFlex.DAL;
using VoiceFlex.DTO;
using VoiceFlex.Models;

namespace VoiceFlex.BLL;

public interface IAccountManager
{
    Task<ICallResult> CreateAccountAsync(AccountDto account);
    Task<ICallResult> GetAccountWithPhoneNumbersAsync(Guid id);
    Task<ICallResult> UpdateAccountAsync(Guid id, AccountUpdateDto accountUpdateDto);
}

public class AccountManager : IAccountManager
{
    private readonly IAccountValidator _accountValidator;
    private readonly IAccountAccessor _accountAccessor;

    public AccountManager(
        IAccountValidator accountValidator,
        IAccountAccessor accountAccessor)
        => (_accountAccessor, _accountValidator)
            = (accountAccessor, accountValidator);

    public async Task<ICallResult> CreateAccountAsync(AccountDto account)
    {
        return _accountValidator
            .DescriptionMustBeValid(account.Description)
            .ErrorFound
            ?? await _accountAccessor.CreateAsync(account);
    }

    public async Task<ICallResult> GetAccountWithPhoneNumbersAsync(Guid id)
    {
        var account = await _accountAccessor.GetAsync(id);
        return _accountValidator
            .FoundInDatabase(account)
            .ErrorFound
            ?? account;
    }

    public async Task<ICallResult> UpdateAccountAsync(Guid id, AccountUpdateDto accountUpdateDto)
    {
        var account = accountUpdateDto.Status == AccountStatus.Active
            ? await _accountAccessor.SetActiveAsync(id)
            : await _accountAccessor.SetSuspendedAsync(id);
        return _accountValidator
            .FoundInDatabase(account)
            .ErrorFound
            ?? account;
    }
}
