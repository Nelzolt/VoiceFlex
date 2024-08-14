using VoiceFlex.DAL;
using VoiceFlex.DTO;

namespace VoiceFlex.BLL;

public interface IAccountManager
{
    Task<AccountDto> CreateAccountAsync(AccountDto account);
    Task<AccountDto> GetAccountWithPhoneNumbersAsync(Guid id);
}

public class AccountManager : IAccountManager
{
    private readonly IAccountAccessor _accountAccessor;

    public AccountManager(IAccountAccessor accountAccessor)
        => _accountAccessor = accountAccessor;

    public async Task<AccountDto> CreateAccountAsync(AccountDto account)
        => await _accountAccessor.CreateAsync(account);

    public async Task<AccountDto> GetAccountWithPhoneNumbersAsync(Guid id)
        => await _accountAccessor.GetAsync(id);
}
