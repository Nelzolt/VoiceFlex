using VoiceFlex.DAL;
using VoiceFlex.Data;
using VoiceFlex.DTO;
using VoiceFlex.Helpers;
using VoiceFlex.Models;

namespace VoiceFlex.BLL;

public interface IPhoneNumberManager
{
    Task<PhoneNumberDto> CreatePhoneNumberAsync(PhoneNumberDto phoneNumber);
    Task<ICallResult> UpdatePhoneNumberAsync(Guid id, PhoneNumberUpdateDto phoneNumberUpdateDto);
    Task DeletePhoneNumberAsync(Guid id);
}

public class PhoneNumberManager : IPhoneNumberManager
{
    private readonly IAccountAccessor _accountAccessor;
    private readonly IPhoneNumberAccessor _phoneNumberAccessor;

    public PhoneNumberManager(IPhoneNumberAccessor phoneNumberAccessor, IAccountAccessor accountAccessor)
        => (_phoneNumberAccessor, _accountAccessor) = (phoneNumberAccessor, accountAccessor);

    public async Task<PhoneNumberDto> CreatePhoneNumberAsync(PhoneNumberDto phoneNumber)
        => await _phoneNumberAccessor.CreateAsync(phoneNumber);

    public async Task<ICallResult> UpdatePhoneNumberAsync(Guid id, PhoneNumberUpdateDto phoneNumberUpdateDto)
    {
        var isAssignAttempt = phoneNumberUpdateDto.AccountId is not null;
        if (isAssignAttempt)
        {
            var account = await _accountAccessor.GetAsync((Guid)phoneNumberUpdateDto.AccountId);
            if (account.Status == AccountStatus.Suspended)
            {
                return new CallError(ErrorCodes.VOICEFLEX_0004);
            }
        }
        return await _phoneNumberAccessor.UpdateAsync(id, phoneNumberUpdateDto);
    }

    public async Task DeletePhoneNumberAsync(Guid id)
        => await _phoneNumberAccessor.DeleteAsync(id);
}
