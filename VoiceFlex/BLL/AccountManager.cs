using VoiceFlex.DAL;
using VoiceFlex.DTO;

namespace VoiceFlex.BLL;

public interface IAccountManager
{
    Task<AccountDto> GetAccountWithPhoneNumbersAsync(Guid id);
}

public class AccountManager : IAccountManager
{
    private readonly IAccountAccessor _accountAccessor;

    public AccountManager(IAccountAccessor accountAccessor)
        => _accountAccessor = accountAccessor;

    public async Task<AccountDto> GetAccountWithPhoneNumbersAsync(Guid id)
        => await _accountAccessor.GetAsync(id);
}
