using VoiceFlex.DTO;

namespace VoiceFlex.BLL;

public interface IErrorManager
{
    public IResult Error(ErrorCodes errorCode);
    public IResult ErrorOrOk(ICallResult callResult);
}

public class ErrorManager : IErrorManager
{
    private readonly Dictionary<ErrorCodes, string> errorList = new()
    {
        { ErrorCodes.VOICEFLEX_0000, "404|A resource with this id could not be found." },
        { ErrorCodes.VOICEFLEX_0001, "400|The number must have at least 1 and not more than 11 characters." },
        { ErrorCodes.VOICEFLEX_0002, "400|This phone number is already assigned to another account." },
        { ErrorCodes.VOICEFLEX_0003, "400|A phone number cannot be assigned to a suspended account." },
        { ErrorCodes.VOICEFLEX_0004, "404|An account with this account id could not be found." },
        { ErrorCodes.VOICEFLEX_0005, "400|The description must have at least 1 and not more than 1023 characters." },
        { ErrorCodes.VOICEFLEX_0006, "400|A phone number with this number already exists." }
    };

    public IResult Error(ErrorCodes errorCode)
    {
        var errorValues = errorList[errorCode].Split('|');
        return Results.Json(new ErrorDto(errorCode, errorValues[1]), statusCode: int.Parse(errorValues[0]));
    }

    public IResult ErrorOrOk(ICallResult callResult)
        => callResult is CallError error ? Error(error.Code) : Results.Ok(callResult);
}

public enum ErrorCodes
{
    VOICEFLEX_0000,
    VOICEFLEX_0001,
    VOICEFLEX_0002,
    VOICEFLEX_0003,
    VOICEFLEX_0004,
    VOICEFLEX_0005,
    VOICEFLEX_0006
}
