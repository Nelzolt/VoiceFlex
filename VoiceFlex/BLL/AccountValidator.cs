using VoiceFlex.DTO;

namespace VoiceFlex.BLL;

public interface IAccountValidator : IValidator
{
    ICallResult NewAccountError(AccountDto account);
}

public class AccountValidator : Validator, IAccountValidator
{
    public AccountValidator() { }

    public ICallResult NewAccountError(AccountDto account)
    {
        // The account description must have at least 1 and not more than 1023 characters
        if (account.Description is null
            || account.Description.Length < 1
            || account.Description.Length > 1023)
        {
            return new CallError(ErrorCodes.VOICEFLEX_0005);
        }
        return null;
    }
}
