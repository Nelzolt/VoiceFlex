using VoiceFlex.DTO;
using VoiceFlex.Models;

namespace VoiceFlex.BLL;

public interface IPhoneNumberValidator : IValidator<PhoneNumberValidator>
{

    IPhoneNumberValidator FoundInDatabase<T>(T entity);
    IPhoneNumberValidator NumberMustBeNew(PhoneNumber phoneNumber);
    IPhoneNumberValidator NumberMustBeValid(string number);
    IPhoneNumberValidator AccountMustBeInDatabase(AccountDto account);
    IPhoneNumberValidator AccountMustBeActive(AccountStatus status);
    IPhoneNumberValidator PhoneNumberMustBeUnassigned(Guid? accountId);
}

public class PhoneNumberValidator : Validator<PhoneNumberValidator>, IPhoneNumberValidator
{
    public PhoneNumberValidator() { }

    public IPhoneNumberValidator FoundInDatabase<T>(T entity)
        => SetErrorIf(entity is null, ErrorCodes.VOICEFLEX_0000);

    public IPhoneNumberValidator NumberMustBeNew(PhoneNumber phoneNumber)
        => SetErrorIf(phoneNumber is not null, ErrorCodes.VOICEFLEX_0006);

    public IPhoneNumberValidator NumberMustBeValid(string number)
        => SetErrorIf(string.IsNullOrEmpty(number) || number.Length > 11, ErrorCodes.VOICEFLEX_0001);

    public IPhoneNumberValidator AccountMustBeInDatabase(AccountDto account)
        => SetErrorIf(account is null, ErrorCodes.VOICEFLEX_0004);

    public IPhoneNumberValidator AccountMustBeActive(AccountStatus status)
        => SetErrorIf(status == AccountStatus.Suspended, ErrorCodes.VOICEFLEX_0003);

    public IPhoneNumberValidator PhoneNumberMustBeUnassigned(Guid? accountId)
        => SetErrorIf(accountId is not null, ErrorCodes.VOICEFLEX_0002);
}
