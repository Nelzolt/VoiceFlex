using Moq;
using VoiceFlex.BLL;
using VoiceFlex.DAL;
using VoiceFlex.Data;
using VoiceFlex.DTO;
using VoiceFlex.Models;

namespace VoiceFlex.Tests.BLL;

public class PhoneNumberManagerTests
{
    private Mock<IPhoneNumberValidator> _mockPhoneNumberValidator;
    private Mock<IPhoneNumberAccessor> _mockPhoneNumberAccessor;
    private Mock<IAccountAccessor> _mockAccountAccessor;
    private PhoneNumberManager _phoneNumberManager;
    private PhoneNumberDto _expectedPhoneNumber;
    private PhoneNumber _phoneNumber;
    private Guid _suspendedAccountId;
    private AccountDto _suspendedAccount;

    [SetUp]
    public void SetUp()
    {
        _mockPhoneNumberValidator = new Mock<IPhoneNumberValidator>();
        _mockPhoneNumberAccessor = new Mock<IPhoneNumberAccessor>();
        _mockAccountAccessor = new Mock<IAccountAccessor>();
        _phoneNumberManager = new PhoneNumberManager(_mockPhoneNumberValidator.Object, _mockPhoneNumberAccessor.Object, _mockAccountAccessor.Object);
        _expectedPhoneNumber = new PhoneNumberDto();
        _suspendedAccountId = Guid.NewGuid();
        _suspendedAccount = new AccountDto
        {
            Id = _suspendedAccountId,
            Status = AccountStatus.Suspended
        };
    }

    [Test]
    public async Task CreatePhoneNumberAsync_Should_Call_PhoneNumberAccessor_CreateAsync_With_Correct_Parameters()
    {
        // Arrange
        _mockPhoneNumberAccessor
            .Setup(accessor => accessor.CreateAsync(It.IsAny<PhoneNumberDto>()))
            .ReturnsAsync(_phoneNumber);

        // Act
        var actualPhoneNumber = await _phoneNumberManager.CreatePhoneNumberAsync(_expectedPhoneNumber);

        // Assert
        _mockPhoneNumberAccessor.Verify(accessor => accessor.CreateAsync(It.Is<PhoneNumberDto>(p => p.Equals(_expectedPhoneNumber))), Times.Once);
        Assert.That(actualPhoneNumber, Is.EqualTo(_phoneNumber));
    }

    [Test]
    public async Task CreatePhoneNumberAsync_Should_Call_PhoneNumberValidator_Error_With_Correct_Parameters()
    {
        // Arrange
        _mockPhoneNumberAccessor
            .Setup(accessor => accessor.CreateAsync(It.IsAny<PhoneNumberDto>()))
            .ReturnsAsync(_phoneNumber);

        // Act
        var actualPhoneNumber = await _phoneNumberManager.CreatePhoneNumberAsync(_expectedPhoneNumber);

        // Assert
        _mockPhoneNumberAccessor.Verify(accessor => accessor.CreateAsync(It.Is<PhoneNumberDto>(p => p.Equals(_expectedPhoneNumber))), Times.Once);
        Assert.That(actualPhoneNumber, Is.EqualTo(_phoneNumber));
    }

    [Test]
    public async Task UpdatePhoneNumberAsync_Should_Call_PhoneNumberAccessor_UpdateAsync_With_Correct_Parameters()
    {
        // Arrange
        var phoneNumberId = Guid.NewGuid();
        var phoneNumberUpdateDto = new PhoneNumberUpdateDto();
        _mockPhoneNumberAccessor
            .Setup(accessor => accessor.UpdateAsync(It.IsAny<Guid>(), It.IsAny<PhoneNumberUpdateDto>()))
            .ReturnsAsync(_phoneNumber);

        // Act
        var actualPhoneNumber = await _phoneNumberManager.UpdatePhoneNumberAsync(phoneNumberId, phoneNumberUpdateDto);

        // Assert
        _mockPhoneNumberAccessor.Verify(accessor => accessor.UpdateAsync(
            It.Is<Guid>(p => p.Equals(phoneNumberId)),
            It.Is<PhoneNumberUpdateDto>(p => p.Equals(phoneNumberUpdateDto))),
            Times.Once);
        Assert.That(actualPhoneNumber, Is.EqualTo(_phoneNumber));
    }

    [Test]
    public async Task UpdatePhoneNumberAsync_Should_Call_AccountAccessor_GetAsync_With_Correct_Parameters()
    {
        // Arrange
        var phoneNumberId = Guid.NewGuid();
        var phoneNumberUpdateDto = new PhoneNumberUpdateDto { AccountId = _suspendedAccountId };
        _mockAccountAccessor
            .Setup(accessor => accessor.GetAsync(It.IsAny<Guid>()))
            .ReturnsAsync(_suspendedAccount);

        // Act
        await _phoneNumberManager.UpdatePhoneNumberAsync(phoneNumberId, phoneNumberUpdateDto);

        // Assert
        _mockAccountAccessor.Verify(accessor => accessor.GetAsync(It.Is<Guid>(p => p.Equals(_suspendedAccountId))), Times.Once);
    }

    [Test]
    public async Task UpdatePhoneNumberAsync_With_Suspended_Account_Should_Return_Error()
    {
        // Arrange
        var phoneNumberId = Guid.NewGuid();
        var phoneNumberUpdateDto = new PhoneNumberUpdateDto { AccountId = _suspendedAccountId };
        _mockAccountAccessor
            .Setup(accessor => accessor.GetAsync(It.IsAny<Guid>()))
            .ReturnsAsync(_suspendedAccount);

        // Act
        var error = await _phoneNumberManager.UpdatePhoneNumberAsync(phoneNumberId, phoneNumberUpdateDto) as CallError;

        // Assert
        Assert.That(error, Is.Not.Null);
        Assert.That(error.Code, Is.EqualTo(ErrorCodes.VOICEFLEX_0004));
    }

    [Test]
    public async Task DeletePhoneNumberAsync_Should_Call_PhoneNumberAccessor_DeleteAsync_With_Correct_Parameters()
    {
        // Arrange
        var phoneNumberId = Guid.NewGuid();

        // Act
        await _phoneNumberManager.DeletePhoneNumberAsync(phoneNumberId);

        // Assert
        _mockPhoneNumberAccessor.Verify(accessor => accessor.DeleteAsync(It.Is<Guid>(p => p.Equals(phoneNumberId))), Times.Once);
    }
}
