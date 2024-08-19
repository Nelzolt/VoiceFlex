using VoiceFlex.BLL;
using VoiceFlex.DTO;
using VoiceFlex.Models;

namespace VoiceFlex.Tests.BLL;

public class PhoneNumberValidatorTests : ValidatorTests
{
    private PhoneNumberValidator _phoneNumberValidator;
    private PhoneNumber _phoneNumber;

    [SetUp]
    public void SetUp()
    {
        _phoneNumberValidator = new PhoneNumberValidator();
    }

    [TestCase(true, true)]
    [TestCase(false, false)]
    public void FoundInDatabase_Should_Return_Correct_Result(bool entityFound, bool isValid)
    {
        // Arrange
        var _phoneNumber = entityFound ? new PhoneNumberDto() : null;

        // Act
        var error = _phoneNumberValidator.FoundInDatabase(_phoneNumber).ErrorFound as CallError;

        // Assert
        AssertValidationResult(error, isValid, ErrorCodes.VOICEFLEX_0000);
    }

    [TestCase(true, true)]
    [TestCase(false, false)]
    public void NumberMustBeNew_Should_Return_Correct_Result(bool numberIsNew, bool isValid)
    {
        // Arrange
        _phoneNumber = numberIsNew ? null : new PhoneNumber();

        // Act
        var error = _phoneNumberValidator.NumberMustBeNew(_phoneNumber).ErrorFound as CallError;

        // Assert
        AssertValidationResult(error, isValid, ErrorCodes.VOICEFLEX_0006);
    }

    [TestCase(null, false)]
    [TestCase("", false)]
    [TestCase("1", true)]
    [TestCase("12345678901", true)]
    [TestCase("123456789012", false)]
    public void NumberMustBeValid_Should_Return_Correct_Result(string number, bool isValid)
    {
        // Act
        var error = _phoneNumberValidator.NumberMustBeValid(number).ErrorFound as CallError;

        // Assert
        AssertValidationResult(error, isValid, ErrorCodes.VOICEFLEX_0001);
    }

    [TestCase(true, true)]
    [TestCase(false, false)]
    public void AccountMustBeInDatabase_Should_Return_Correct_Result(bool accountFound, bool isValid)
    {
        // Arrange
        var account = accountFound ? new AccountDto() : null;

        // Act
        var error = _phoneNumberValidator.AccountMustBeInDatabase(account).ErrorFound as CallError;

        // Assert
        AssertValidationResult(error, isValid, ErrorCodes.VOICEFLEX_0004);
    }

    [TestCase(AccountStatus.Active, true)]
    [TestCase(AccountStatus.Suspended, false)]
    public void AccountMustBeActive_Should_Return_Correct_Result(AccountStatus status, bool isValid)
    {
        // Act
        var error = _phoneNumberValidator.AccountMustBeActive(status).ErrorFound as CallError;

        // Assert
        AssertValidationResult(error, isValid, ErrorCodes.VOICEFLEX_0003);
    }

    [TestCase(true, true)]
    [TestCase(false, false)]
    public void PhoneNumberMustBeUnassigned_Should_Return_Correct_Result(bool guidIsNull, bool isValid)
    {
        // Arrange
        Guid? accountId = guidIsNull ? null : Guid.NewGuid();

        // Act
        var error = _phoneNumberValidator.PhoneNumberMustBeUnassigned(accountId).ErrorFound as CallError;

        // Assert
        AssertValidationResult(error, isValid, ErrorCodes.VOICEFLEX_0002);
    }
}
