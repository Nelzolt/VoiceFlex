using VoiceFlex.DAL;
using VoiceFlex.DTO;
using VoiceFlex.Models;

namespace VoiceFlex.BLL;

public interface IPhoneNumberManager
{
    Task<ICallResult> CreatePhoneNumberAsync(PhoneNumberDto phoneNumber);
    Task<ICallResult> UpdatePhoneNumberAsync(Guid id, PhoneNumberUpdateDto phoneNumberUpdateDto);
    Task<ICallResult> DeletePhoneNumberAsync(Guid id);
}

public class PhoneNumberManager : IPhoneNumberManager
{
    private readonly IPhoneNumberValidator _phoneNumberValidator;
    private readonly IAccountAccessor _accountAccessor;
    private readonly IPhoneNumberAccessor _phoneNumberAccessor;

    public PhoneNumberManager(
        IPhoneNumberValidator phoneNumberValidator,
        IPhoneNumberAccessor phoneNumberAccessor,
        IAccountAccessor accountAccessor)
        => (_phoneNumberValidator, _phoneNumberAccessor, _accountAccessor)
        = (phoneNumberValidator, phoneNumberAccessor, accountAccessor);

    public async Task<ICallResult> CreatePhoneNumberAsync(PhoneNumberDto phoneNumber)
    {
        if (_phoneNumberValidator.Error(phoneNumber, out var callError))
        {
            return callError;
        }
        return await _phoneNumberAccessor.CreateAsync(phoneNumber);
    }

    public async Task<ICallResult> UpdatePhoneNumberAsync(Guid id, PhoneNumberUpdateDto phoneNumberUpdateDto)
    {
        var isAssignAttempt = phoneNumberUpdateDto.AccountId is not null;
        if (isAssignAttempt)
        {
            var account = await _accountAccessor.GetAsync((Guid)phoneNumberUpdateDto.AccountId) as AccountDto;
            if (account is null)
            {
                return new CallError(ErrorCodes.VOICEFLEX_0005);
            }
            if (account.Status == AccountStatus.Suspended)
            {
                return new CallError(ErrorCodes.VOICEFLEX_0004);
            }
        }
        return await _phoneNumberAccessor.UpdateAsync(id, phoneNumberUpdateDto);
    }

    public async Task<ICallResult> DeletePhoneNumberAsync(Guid id)
        => await _phoneNumberAccessor.DeleteAsync(id);
}
