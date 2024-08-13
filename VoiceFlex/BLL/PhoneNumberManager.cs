using VoiceFlex.DAL;
using VoiceFlex.Models;

namespace VoiceFlex.BLL;

public interface IPhoneNumberManager
{
    Task<List<PhoneNumber>> ListPhoneNumbersAsync();
}

public class PhoneNumberManager : IPhoneNumberManager
{
    private readonly IPhoneNumberAccessor _phoneNumberAccessor;

    public PhoneNumberManager(IPhoneNumberAccessor phoneNumberAccessor)
        => _phoneNumberAccessor = phoneNumberAccessor;

    public async Task<List<PhoneNumber>> ListPhoneNumbersAsync()
        => await _phoneNumberAccessor.ListAsync();
}
