using VoiceFlex.DTO;

namespace VoiceFlex.Helpers;

public interface IErrorManager
{
    public IResult Error(ErrorCodes errorCode);
}

public class ErrorManager : IErrorManager
{
    private readonly Dictionary<ErrorCodes, string> errorList = new()
    {
        { ErrorCodes.VOICEFLEX_0001, "404|A resource with this id could not be found." },
        { ErrorCodes.VOICEFLEX_0002, "400|The phone number must be between 1 and 11 characters." },
        { ErrorCodes.VOICEFLEX_0003, "400|The phone number is already assigned to someone else." },
        { ErrorCodes.VOICEFLEX_0004, "400|A phone number cannot be assigned to a suspended account." }
    };

    public IResult Error(ErrorCodes errorCode)
    {
        var errorValues = errorList[errorCode].Split('|');
        return Results.Json(new ErrorDto(errorCode, errorValues[1]), statusCode: int.Parse(errorValues[0]));
    }
}

public enum ErrorCodes
{
    VOICEFLEX_0001,
    VOICEFLEX_0002,
    VOICEFLEX_0003,
    VOICEFLEX_0004
}
