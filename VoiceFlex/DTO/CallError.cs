using VoiceFlex.BLL;

namespace VoiceFlex.DTO;

public class CallError : ICallResult
{
    public ErrorCodes Code { get; set; }

    public CallError(ErrorCodes errorCode)
        => Code = errorCode;
}
