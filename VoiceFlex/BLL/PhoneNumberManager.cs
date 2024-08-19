using VoiceFlex.DAL;
using VoiceFlex.DTO;
using VoiceFlex.Models;

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
        var existingPhoneNumber = await _phoneNumberAccessor.GetByNumberAsync(phoneNumber.Number);

        return _phoneNumberValidator
            .NumberMustBeNew(existingPhoneNumber)
            .NumberMustBeValid(phoneNumber.Number)
            .ErrorFound
            ?? await _phoneNumberAccessor.CreateAsync(phoneNumber);
    }

    public async Task<ICallResult> AssignUnassignPhoneNumberAsync(Guid id, PhoneNumberUpdateDto phoneNumberUpdate)
    {
        var isAssignAttempt = phoneNumberUpdate.AccountId is not null && phoneNumberUpdate.AccountId != Guid.Empty;
        var dbPhoneNumber = await _phoneNumberAccessor.GetAsync(id);

        return isAssignAttempt
            ? await AssignPhoneNumber(dbPhoneNumber)
            : await UnassignPhoneNumber(dbPhoneNumber);

        async Task <ICallResult> AssignPhoneNumber(PhoneNumber dbPhoneNumber)
        {
            var dbAccount = await _accountAccessor.GetAsync((Guid)phoneNumberUpdate.AccountId);
            return _phoneNumberValidator
                .FoundInDatabase(dbPhoneNumber)
                .AccountMustBeInDatabase(dbAccount)
                .AccountMustBeActive(dbAccount?.Status)
                .PhoneNumberMustBeUnassigned(dbPhoneNumber?.AccountId)
                .ErrorFound
                ?? await _phoneNumberAccessor.AssignAsync(dbPhoneNumber, dbAccount.Id);
        }

        async Task<ICallResult> UnassignPhoneNumber(PhoneNumber dbPhoneNumber)
        {
            return _phoneNumberValidator
                .FoundInDatabase(dbPhoneNumber)
                .ErrorFound
                ?? await _phoneNumberAccessor.UnassignAsync(dbPhoneNumber);
        }
    }

    public async Task<ICallResult> DeletePhoneNumberAsync(Guid id)
    {
        var dbPhoneNumber = await _phoneNumberAccessor.DeleteAsync(id);
        return _phoneNumberValidator
            .FoundInDatabase(dbPhoneNumber)
            .ErrorFound
            ?? dbPhoneNumber;
    }
}
