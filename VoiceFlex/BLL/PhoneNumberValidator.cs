using VoiceFlex.DAL;
using VoiceFlex.DTO;
using VoiceFlex.Models;

namespace VoiceFlex.BLL;

public interface IPhoneNumberValidator : IValidator
{
    Task<ICallResult> NewPhoneNumberErrorsAsync(PhoneNumberDto phoneNumber);
    ICallResult AssignPhoneNumberErrors(PhoneNumber phoneNumber, AccountDto account);
}

public class PhoneNumberValidator : Validator, IPhoneNumberValidator
{
    private readonly IPhoneNumberAccessor _phoneNumberAccessor;
    private ICallResult _error;

    public PhoneNumberValidator(IPhoneNumberAccessor phoneNumberAccessor)
        => _phoneNumberAccessor = phoneNumberAccessor;

    public async Task<ICallResult> NewPhoneNumberErrorsAsync(PhoneNumberDto phoneNumber)
    {
        // The phone number must have at least 1 and not more than 11 characters
        if (phoneNumber.Number is null
            || phoneNumber.Number.Length < 1
            || phoneNumber.Number.Length > 11)
        {
            return new CallError(ErrorCodes.VOICEFLEX_0001);
        }

        // A phone number with this number already exists
        if (await _phoneNumberAccessor.GetByNumberAsync(phoneNumber.Number) is not null)
        {
            return new CallError(ErrorCodes.VOICEFLEX_0006);
        }
        return null;
    }

    public ICallResult AssignPhoneNumberErrors(PhoneNumber phoneNumber, AccountDto account)
    {
        if (NotFoundError(phoneNumber, out _error))
        {
            // A phone number with this id could not be found
            return _error;
        }

        if (NotFoundError(account, ErrorCodes.VOICEFLEX_0004, out _error))
        {
            // An account with this account id could not be found
            return _error;
        }

        if (account.Status == AccountStatus.Suspended)
        {
            // A phone number cannot be assigned to a suspended account
            return new CallError(ErrorCodes.VOICEFLEX_0003);
        }

        if (phoneNumber.AccountId is not null)
        {
            // This phone number is already assigned to another account
            return new CallError(ErrorCodes.VOICEFLEX_0002);
        }
        return null;
    }
}
