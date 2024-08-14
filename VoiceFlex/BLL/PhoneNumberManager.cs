using VoiceFlex.DAL;
using VoiceFlex.DTO;

namespace VoiceFlex.BLL;

public interface IPhoneNumberManager
{
    Task<PhoneNumberDto> CreatePhoneNumberAsync(PhoneNumberDto phoneNumber);
    Task DeletePhoneNumberAsync(Guid id);
}

public class PhoneNumberManager : IPhoneNumberManager
{
    private readonly IPhoneNumberAccessor _phoneNumberAccessor;

    public PhoneNumberManager(IPhoneNumberAccessor phoneNumberAccessor)
        => _phoneNumberAccessor = phoneNumberAccessor;

    public async Task<PhoneNumberDto> CreatePhoneNumberAsync(PhoneNumberDto phoneNumber)
        => await _phoneNumberAccessor.CreateAsync(phoneNumber);

    public async Task DeletePhoneNumberAsync(Guid id)
        => await _phoneNumberAccessor.DeleteAsync(id);
}
