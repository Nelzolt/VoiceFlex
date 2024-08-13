using VoiceFlex.DAL;
using VoiceFlex.DTO;

namespace VoiceFlex.BLL;

public interface IPhoneNumberManager
{
    Task<List<PhoneNumberDto>> ListPhoneNumbersAsync();
}

public class PhoneNumberManager : IPhoneNumberManager
{
    private readonly IPhoneNumberAccessor _phoneNumberAccessor;

    public PhoneNumberManager(IPhoneNumberAccessor phoneNumberAccessor)
        => _phoneNumberAccessor = phoneNumberAccessor;

    public async Task<List<PhoneNumberDto>> ListPhoneNumbersAsync()
        => await _phoneNumberAccessor.ListAsync();
}
