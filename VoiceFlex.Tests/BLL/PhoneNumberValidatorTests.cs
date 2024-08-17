using Moq;
using VoiceFlex.BLL;
using VoiceFlex.DAL;
using VoiceFlex.DTO;
using VoiceFlex.Models;

namespace VoiceFlex.Tests.BLL;

public class PhoneNumberValidatorTests
{
    private Mock<IPhoneNumberAccessor> _mockPhoneNumberAccessor;
    private Mock<IAccountAccessor> _mockAccountAccessor;
    private PhoneNumberValidator _phoneNumberValidator;
    private PhoneNumberDto _testPhoneNumber;
    private CallError _error;


    [SetUp]
    public void SetUp()
    {
        _mockPhoneNumberAccessor = new Mock<IPhoneNumberAccessor>();
        _mockAccountAccessor = new Mock<IAccountAccessor>();
        _phoneNumberValidator = new PhoneNumberValidator(_mockPhoneNumberAccessor.Object, _mockAccountAccessor.Object);
        _testPhoneNumber = new PhoneNumberDto();
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase("123456789012")]
    public async Task Invalid_Data_Should_Return_Correct_Error_Code(string number)
    {
        // Arrange
        _testPhoneNumber.Number = number;

        // Act
        _error = await _phoneNumberValidator.NewPhoneNumberErrorAsync(_testPhoneNumber);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(_error, Is.Not.Null);
            Assert.That(_error.Code, Is.EqualTo(ErrorCodes.VOICEFLEX_0001));
        });
    }

    [TestCase("1")]
    [TestCase("12345678901")]
    public async Task Valid_Data_Should_Not_Return_Error(string number)
    {
        // Arrange
        _testPhoneNumber.Number = number;

        // Act
        var error = await _phoneNumberValidator.NewPhoneNumberErrorAsync(_testPhoneNumber);

        // Assert
        Assert.That(error, Is.Null);
    }
}
