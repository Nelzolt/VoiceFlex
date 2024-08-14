using VoiceFlex.DAL;
using VoiceFlex.DTO;

namespace VoiceFlex.BLL;

public interface IPhoneNumberManager
{
    Task<PhoneNumberDto> CreatePhoneNumberAsync(PhoneNumberDto phoneNumber);
}

public class PhoneNumberManager : IPhoneNumberManager
{
    private readonly IPhoneNumberAccessor _phoneNumberAccessor;

    public PhoneNumberManager(IPhoneNumberAccessor phoneNumberAccessor)
        => _phoneNumberAccessor = phoneNumberAccessor;

    public async Task<PhoneNumberDto> CreatePhoneNumberAsync(PhoneNumberDto phoneNumber)
        => await _phoneNumberAccessor.CreateAsync(phoneNumber);
}
