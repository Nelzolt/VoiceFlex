using VoiceFlex.DTO;

namespace VoiceFlex.BLL;

public interface IValidator
{
    ICallResult ErrorFound { get; }
}

public class Validator<T> : IValidator where T : Validator<T>
{
    public ICallResult ErrorFound { get; private set; }

    protected T SetErrorIf(bool condition, ErrorCodes errorCode)
    {
        if (ErrorFound is null && condition)
        {
            ErrorFound = new CallError(errorCode);
        }
        return this as T;
    }
}
