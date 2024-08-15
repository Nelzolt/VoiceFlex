using VoiceFlex.Helpers;

namespace VoiceFlex.Data;

public class CallError : ICallResult
{
    public ErrorCodes Code { get; set; }

    public CallError(ErrorCodes errorCode)
        => Code = errorCode;
}
