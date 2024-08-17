using VoiceFlex.DTO;

namespace VoiceFlex.BLL;

public interface IAccountValidator
{
    ICallResult NewAccountError(AccountDto account);
}

public class AccountValidator : IAccountValidator
{
    public AccountValidator() { }

    public ICallResult NewAccountError(AccountDto account)
        // The description must have at least 1 and not more than 1023 characters
        => account.Description is null
            || account.Description.Length < 1
            || account.Description.Length > 1023
            ? new CallError(ErrorCodes.VOICEFLEX_0005)
            : null;
}
