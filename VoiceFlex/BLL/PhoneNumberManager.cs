using VoiceFlex.DAL;
using VoiceFlex.DTO;

namespace VoiceFlex.BLL;

public interface IPhoneNumberManager
{
    Task<ICallResult> CreatePhoneNumberAsync(PhoneNumberDto phoneNumber);
    Task<ICallResult> AssignUnassignPhoneNumberAsync(Guid id, PhoneNumberUpdateDto phoneNumberUpdateDto);
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
        => await _phoneNumberValidator.NewPhoneNumberError(phoneNumber)
            ?? await _phoneNumberAccessor.CreateAsync(phoneNumber);

    public async Task<ICallResult> AssignUnassignPhoneNumberAsync(Guid id, PhoneNumberUpdateDto phoneNumberUpdate)
    {
        phoneNumberUpdate.Id = id;
        var isAssignAttempt = phoneNumberUpdate.AccountId is not null;
        var dbPhoneNumber = await _phoneNumberAccessor.GetAsync(phoneNumberUpdate.Id);
        var error = isAssignAttempt
            ? await _phoneNumberValidator.AssignPhoneNumberError(dbPhoneNumber, phoneNumberUpdate)
            : _phoneNumberValidator.UnassignPhoneNumberError(dbPhoneNumber);
        return error
            ?? await _phoneNumberAccessor.AssignUnassignAsync(dbPhoneNumber, phoneNumberUpdate.AccountId);
    }

    public async Task<ICallResult> DeletePhoneNumberAsync(Guid id)
        => await _phoneNumberAccessor.DeleteAsync(id)
            ?? (ICallResult)new CallError(ErrorCodes.VOICEFLEX_0000);
}
