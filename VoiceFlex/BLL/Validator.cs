using VoiceFlex.DTO;

namespace VoiceFlex.BLL;

public interface IValidator
{
    bool NotFoundError(object entity, out ICallResult error);
    ICallResult NotFoundError(object entity);
}

public class Validator : IValidator
{
    public bool NotFoundError(object entity, out ICallResult error)
        => NotFoundError(entity, ErrorCodes.VOICEFLEX_0000, out error);

    public bool NotFoundError(object entity, ErrorCodes errorCode, out ICallResult error)
    {
        error = entity is null ? new CallError(errorCode) : null;
        return error is not null;
    }

    public ICallResult NotFoundError(object entity)
    {
        if (entity is null)
        {
            return new CallError(ErrorCodes.VOICEFLEX_0000);
        }
        return null;
    }
}
