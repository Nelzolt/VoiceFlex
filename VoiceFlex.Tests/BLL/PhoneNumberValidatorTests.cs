using VoiceFlex.BLL;
using VoiceFlex.DTO;

namespace VoiceFlex.Tests.BLL;

public class PhoneNumberValidatorTests
{
    private PhoneNumberValidator _phoneNumberValidator;
    private PhoneNumberDto _testPhoneNumber;
    private CallError _error;


    [SetUp]
    public void SetUp()
    {
        _phoneNumberValidator = new PhoneNumberValidator();
        _testPhoneNumber = new PhoneNumberDto();
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase("123456789012")]
    public void Invalid_Data_Should_Return_Correct_Error_Code(string number)
    {
        // Arrange
        _testPhoneNumber.Number = number;

        // Act
        var hasError = _phoneNumberValidator.Error(_testPhoneNumber, out _error);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(hasError, Is.True);
            Assert.That(_error, Is.Not.Null);
            Assert.That(_error.Code, Is.EqualTo(ErrorCodes.VOICEFLEX_0002));
        });
    }

    [TestCase("1")]
    [TestCase("12345678901")]
    public void Valid_Data_Should_Not_Return_Error(string number)
    {
        // Arrange
        _testPhoneNumber.Number = number;

        // Act
        var hasError = _phoneNumberValidator.Error(_testPhoneNumber, out _error);

        // Assert
        Assert.That(hasError, Is.False);
    }
}
