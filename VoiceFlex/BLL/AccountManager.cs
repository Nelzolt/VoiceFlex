using VoiceFlex.DAL;
using VoiceFlex.DTO;

namespace VoiceFlex.BLL;

public interface IAccountManager
{
    Task<AccountDto> GetAccountWithPhoneNumbersAsync(Guid id);
}

public class AccountManager : IAccountManager
{
    private readonly IPhoneNumberAccessor _phoneNumberAccessor;

    public AccountManager(IPhoneNumberAccessor phoneNumberAccessor)
        => _phoneNumberAccessor = phoneNumberAccessor;

    public async Task<AccountDto> GetAccountWithPhoneNumbersAsync(Guid id)
    {
        return await Task.FromResult<AccountDto>(default);
    }
        //=> await _phoneNumberAccessor.ListAsync();
}
