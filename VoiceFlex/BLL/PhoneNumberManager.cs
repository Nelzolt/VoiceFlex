using VoiceFlex.DAL;
using VoiceFlex.DTO;
using VoiceFlex.Models;

namespace VoiceFlex.BLL;

public interface IPhoneNumberManager
{
    Task<PhoneNumberDto> CreatePhoneNumberAsync(PhoneNumberDto phoneNumber);
    Task<PhoneNumber> UpdatePhoneNumberAsync(Guid id, PhoneNumberUpdateDto phoneNumberUpdateDto);
    Task DeletePhoneNumberAsync(Guid id);
}

public class PhoneNumberManager : IPhoneNumberManager
{
    private readonly IPhoneNumberAccessor _phoneNumberAccessor;

    public PhoneNumberManager(IPhoneNumberAccessor phoneNumberAccessor)
        => _phoneNumberAccessor = phoneNumberAccessor;

    public async Task<PhoneNumberDto> CreatePhoneNumberAsync(PhoneNumberDto phoneNumber)
        => await _phoneNumberAccessor.CreateAsync(phoneNumber);

    public async Task<PhoneNumber> UpdatePhoneNumberAsync(Guid id, PhoneNumberUpdateDto phoneNumberUpdateDto)
        => await _phoneNumberAccessor.UpdateAsync(id, phoneNumberUpdateDto);

    public async Task DeletePhoneNumberAsync(Guid id)
        => await _phoneNumberAccessor.DeleteAsync(id);
}
