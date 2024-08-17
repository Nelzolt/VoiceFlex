using VoiceFlex.DAL;
using VoiceFlex.DTO;
using VoiceFlex.Models;

namespace VoiceFlex.BLL;

public interface IPhoneNumberValidator
{
    Task<CallError> NewPhoneNumberErrorAsync(PhoneNumberDto phoneNumber);
    Task<CallError> AssignPhoneNumberError(PhoneNumber phoneNumber, PhoneNumberUpdateDto phoneNumberUpdate);
    CallError UnassignPhoneNumberError(PhoneNumber phoneNumber);
}

public class PhoneNumberValidator : IPhoneNumberValidator
{
    private readonly IPhoneNumberAccessor _phoneNumberAccessor;
    private readonly IAccountAccessor _accountAccessor;

    public PhoneNumberValidator(
        IPhoneNumberAccessor phoneNumberAccessor,
        IAccountAccessor accountAccessor)
        => (_phoneNumberAccessor, _accountAccessor)
            = (phoneNumberAccessor, accountAccessor);

    public async Task<CallError> NewPhoneNumberErrorAsync(PhoneNumberDto phoneNumber)
    {
        // The number must have at least 1 and not more than 11 characters
        var error = phoneNumber.Number is null
            || phoneNumber.Number.Length < 1
            || phoneNumber.Number.Length > 11
            ? new CallError(ErrorCodes.VOICEFLEX_0001)
            : null;
        if (error is not null) { return error; }

        // A phone number with this number already exists
        return await _phoneNumberAccessor.GetByNumberAsync(phoneNumber.Number) is not null
            ? new CallError(ErrorCodes.VOICEFLEX_0006)
            : null;
    }

    public async Task<CallError> AssignPhoneNumberError(PhoneNumber phoneNumber, PhoneNumberUpdateDto phoneNumberUpdate)
    {
        // A phone number with this id could not be found
        if (phoneNumber is null)
        {
            return new CallError(ErrorCodes.VOICEFLEX_0000);
        }

        var account = await _accountAccessor.GetAsync((Guid)phoneNumberUpdate.AccountId) as AccountDto;

        // An account with this account id could not be found
        if (account is null)
        {
            return new CallError(ErrorCodes.VOICEFLEX_0004);
        }

        // A phone number cannot be assigned to a suspended account
        if (account.Status == AccountStatus.Suspended)
        {
            return new CallError(ErrorCodes.VOICEFLEX_0003);
        }

        // This phone number is already assigned to someone else
        if (phoneNumberUpdate.AccountId is not null)
        {
            return new CallError(ErrorCodes.VOICEFLEX_0002);
        }
        return null;
    }

    public CallError UnassignPhoneNumberError(PhoneNumber phoneNumber)
        => phoneNumber is null
            ? new CallError(ErrorCodes.VOICEFLEX_0000)
            : null;
}
