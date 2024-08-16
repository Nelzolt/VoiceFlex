using VoiceFlex.Data;
using VoiceFlex.DTO;

namespace VoiceFlex.BLL;

public interface IAccountValidator
{
    bool Error(AccountDto account, out CallError error);
}

public class AccountValidator : IAccountValidator
{
    public AccountValidator() { }

    public bool Error(AccountDto account, out CallError error)
    {
        error = account.Description is null
            || account.Description.Length < 1
            || account.Description.Length > 1023
            ? new CallError(ErrorCodes.VOICEFLEX_0006)
            : null;
        return error is not null;
    }
}
