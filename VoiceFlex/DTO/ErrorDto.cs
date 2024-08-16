using VoiceFlex.BLL;

namespace VoiceFlex.DTO;

public class ErrorDto
{
    public string Code { get; set; }
    public string Message { get; set; }

    public ErrorDto() { }

    public ErrorDto(ErrorCodes code, string message)
    {
        Code = code.ToString();
        Message = message;
    }
}
