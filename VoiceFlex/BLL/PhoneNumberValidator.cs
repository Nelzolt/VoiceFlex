using VoiceFlex.DTO;

namespace VoiceFlex.BLL;

public interface IPhoneNumberValidator
{
    bool Error(PhoneNumberDto phoneNumber, out CallError error);
}

public class PhoneNumberValidator : IPhoneNumberValidator
{
    public PhoneNumberValidator() { }

    public bool Error(PhoneNumberDto phoneNumber, out CallError error)
    {
        error = phoneNumber.Number is null
            || phoneNumber.Number.Length < 1
            || phoneNumber.Number.Length > 11
            ? new CallError(ErrorCodes.VOICEFLEX_0002)
            : null;
        return error is not null;
    }
}
