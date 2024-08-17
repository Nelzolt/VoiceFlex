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
        => _accountValidator.NewAccountError(account)
            ?? await _accountAccessor.CreateAsync(account);

    public async Task<ICallResult> GetAccountWithPhoneNumbersAsync(Guid id)
        => await _accountAccessor.GetAsync(id)
            ?? (ICallResult)new CallError(ErrorCodes.VOICEFLEX_0000);

    public async Task<ICallResult> UpdateAccountAsync(Guid id, AccountUpdateDto accountUpdateDto)
        => accountUpdateDto.Status == AccountStatus.Active
            ? await _accountAccessor.SetActiveAsync(id)
            : await _accountAccessor.SetSuspendedAsync(id)
            ?? (ICallResult)new CallError(ErrorCodes.VOICEFLEX_0000);
}
