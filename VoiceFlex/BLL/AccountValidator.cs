namespace VoiceFlex.BLL;

public interface IAccountValidator : IValidator
{
    IAccountValidator FoundInDatabase<T>(T entity);
    IAccountValidator DescriptionMustBeValid(string description);
}

public class AccountValidator : Validator<AccountValidator>, IAccountValidator
{
    public AccountValidator() { }

    public IAccountValidator FoundInDatabase<T>(T entity)
        => SetErrorIf(entity is null, ErrorCodes.VOICEFLEX_0000);

    public IAccountValidator DescriptionMustBeValid(string description)
        => SetErrorIf(string.IsNullOrEmpty(description) || description.Length > 1023, ErrorCodes.VOICEFLEX_0005);
}
