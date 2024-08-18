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
    {
        return await _phoneNumberValidator.NewPhoneNumberErrorsAsync(phoneNumber)
            ?? await _phoneNumberAccessor.CreateAsync(phoneNumber);
    }

    public async Task<ICallResult> AssignUnassignPhoneNumberAsync(Guid id, PhoneNumberUpdateDto phoneNumberUpdate)
    {
        var dbPhoneNumber = await _phoneNumberAccessor.GetAsync(id);

        return await ErrorCheckResult(isAssignAttempt: phoneNumberUpdate.AccountId is not null)
            ?? await _phoneNumberAccessor.AssignUnassignAsync(dbPhoneNumber, phoneNumberUpdate.AccountId);

        async Task<ICallResult> ErrorCheckResult(bool isAssignAttempt)
        {
            if (isAssignAttempt)
            {
                var account = await _accountAccessor.GetAsync((Guid)phoneNumberUpdate.AccountId);
                return _phoneNumberValidator.AssignPhoneNumberErrors(dbPhoneNumber, account);
            }
            return _phoneNumberValidator.NotFoundError(dbPhoneNumber);
        }
    }

    public async Task<ICallResult> DeletePhoneNumberAsync(Guid id)
    {
        var dbPhoneNumber = await _phoneNumberAccessor.DeleteAsync(id);
        return _phoneNumberValidator.NotFoundError(dbPhoneNumber) ?? dbPhoneNumber;
    }
}
