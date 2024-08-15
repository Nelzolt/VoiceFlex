using VoiceFlex.DTO;

namespace VoiceFlex.Helpers;

public interface IErrorManager
{
    public IResult Throw(ErrorCodes errorCode);
}

public class ErrorManager : IErrorManager
{
    private readonly Dictionary<ErrorCodes, string> errorList = new()
    {
        { ErrorCodes.VOICEFLEX_0001, "404|A resource with this id could not be found." },
        { ErrorCodes.VOICEFLEX_0002, "" }
    };


    public IResult Throw(ErrorCodes errorCode)
    {
        var errorValues = errorList[errorCode].Split('|');
        return Results.Json(new ErrorDto(errorCode, errorValues[1]), statusCode: int.Parse(errorValues[0]));
    }
}

public enum ErrorCodes
{
    VOICEFLEX_0001,
    VOICEFLEX_0002
}
