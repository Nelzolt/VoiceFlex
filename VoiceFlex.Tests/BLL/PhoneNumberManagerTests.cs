using Moq;
using VoiceFlex.BLL;
using VoiceFlex.DAL;
using VoiceFlex.DTO;
using VoiceFlex.Models;

namespace VoiceFlex.Tests.BLL;

public class PhoneNumberManagerTests
{
    private Mock<IPhoneNumberValidator> _mockPhoneNumberValidator;
    private Mock<IPhoneNumberAccessor> _mockPhoneNumberAccessor;
    private Mock<IAccountAccessor> _mockAccountAccessor;
    private PhoneNumberManager _phoneNumberManager;
    private PhoneNumberDto _phoneNumberDto;
    private PhoneNumber _phoneNumber;
    private Guid _accountId;
    private AccountDto _account;
    private Guid _phoneNumberId;
    private Guid _suspendedAccountId;

    [SetUp]
    public void SetUp()
    {
        _mockPhoneNumberValidator = new Mock<IPhoneNumberValidator>();
        _mockPhoneNumberAccessor = new Mock<IPhoneNumberAccessor>();
        _mockAccountAccessor = new Mock<IAccountAccessor>();
        _phoneNumberManager = new PhoneNumberManager(_mockPhoneNumberValidator.Object, _mockPhoneNumberAccessor.Object, _mockAccountAccessor.Object);
        _phoneNumberDto = new PhoneNumberDto();
        _accountId = Guid.NewGuid();
        _phoneNumberId = Guid.NewGuid();
        _phoneNumberDto = new PhoneNumberDto
        {
            Number = "1234"
        };
        _phoneNumber = new PhoneNumber
        {
            Id = _phoneNumberId,
            Number = "1234"
        };
        _account = new AccountDto
        {
            Id = _accountId,
            Status = AccountStatus.Active
        };
        _suspendedAccountId = Guid.NewGuid();
    }

    [Test]
    public async Task CreatePhoneNumberAsync_Should_Call_Accessor_And_Validator_With_Correct_Parameters()
    {
        // Arrange
        _mockPhoneNumberAccessor
            .Setup(accessor => accessor.GetByNumberAsync(It.IsAny<string>()))
            .ReturnsAsync(_phoneNumber);
        _mockPhoneNumberValidator
            .Setup(v => v.NumberMustBeNew(It.IsAny<PhoneNumber>()))
            .Returns(_mockPhoneNumberValidator.Object);
        _mockPhoneNumberValidator
            .Setup(v => v.NumberMustBeValid(It.IsAny<string>()))
            .Returns(_mockPhoneNumberValidator.Object);
        _mockPhoneNumberAccessor
            .Setup(accessor => accessor.CreateAsync(It.IsAny<PhoneNumberDto>()))
            .ReturnsAsync(_phoneNumber);

        // Act
        var newPhoneNumber = await _phoneNumberManager.CreatePhoneNumberAsync(_phoneNumberDto);

        // Assert
        _mockPhoneNumberAccessor.Verify(a => a.GetByNumberAsync(It.Is<string>(p => p.Equals(_phoneNumberDto.Number))), Times.Once);
        _mockPhoneNumberValidator.Verify(a => a.NumberMustBeNew(It.Is<PhoneNumber>(p => p.Equals(_phoneNumber))), Times.Once);
        _mockPhoneNumberValidator.Verify(a => a.NumberMustBeValid(It.Is<string>(p => p.Equals(_phoneNumberDto.Number))), Times.Once);
        _mockPhoneNumberAccessor.Verify(a => a.CreateAsync(It.Is<PhoneNumberDto>(p => p.Equals(_phoneNumberDto))), Times.Once);
        Assert.That(newPhoneNumber, Is.EqualTo(_phoneNumber));
    }

    [Test]
    public async Task DeletePhoneNumberAsync_Should_Call_Accessor_And_Validator_With_Correct_Parameters()
    {
        // Arrange
        var phoneNumberId = Guid.NewGuid();
        _mockPhoneNumberAccessor
            .Setup(accessor => accessor.DeleteAsync(It.IsAny<Guid>()))
            .ReturnsAsync(_phoneNumber);
        _mockPhoneNumberValidator
            .Setup(v => v.FoundInDatabase(It.IsAny<PhoneNumber>()))
            .Returns(_mockPhoneNumberValidator.Object);

        // Act
        var deletedPhoneNumber = await _phoneNumberManager.DeletePhoneNumberAsync(phoneNumberId);

        // Assert
        _mockPhoneNumberAccessor.Verify(a => a.DeleteAsync(It.Is<Guid>(p => p.Equals(phoneNumberId))), Times.Once);
        Assert.That(deletedPhoneNumber, Is.EqualTo(_phoneNumber));
    }

    //[Test]
    //public async Task CreatePhoneNumberAsync_Should_Call_PhoneNumberAccessor_CreateAsync_With_Correct_Parameters()
    //{
    //    // Arrange
    //    _mockPhoneNumberAccessor
    //        .Setup(accessor => accessor.CreateAsync(It.IsAny<PhoneNumberDto>()))
    //        .ReturnsAsync(_phoneNumber);

    //    // Act
    //    var actualPhoneNumber = await _phoneNumberManager.CreatePhoneNumberAsync(_expectedPhoneNumber);

    //    // Assert
    //    _mockPhoneNumberAccessor.Verify(accessor => accessor.CreateAsync(It.Is<PhoneNumberDto>(p => p.Equals(_expectedPhoneNumber))), Times.Once);
    //    Assert.That(actualPhoneNumber, Is.EqualTo(_phoneNumber));
    //}

    //[Test]
    //public async Task CreatePhoneNumberAsync_Should_Call_PhoneNumberValidator_Error_With_Correct_Parameters()
    //{
    //    // Arrange
    //    var mockError = new CallError(ErrorCodes.VOICEFLEX_0001);
    //    _mockPhoneNumberValidator
    //        .Setup(v => v.NewPhoneNumberError(It.IsAny<PhoneNumberDto>()))
    //        .Returns(mockError);

    //    // Act
    //    var error = await _phoneNumberManager.CreatePhoneNumberAsync(_expectedPhoneNumber) as CallError;

    //    // Assert
    //    Assert.That(error, Is.Not.Null);
    //    Assert.That(error.Code, Is.EqualTo(ErrorCodes.VOICEFLEX_0001));
    //}

    //[Test]
    //public async Task UpdatePhoneNumberAsync_Should_Call_AccountAccessor_GetAsync_With_Correct_Parameters()
    //{
    //    // Arrange
    //    var phoneNumberId = Guid.NewGuid();
    //    var phoneNumberUpdateDto = new PhoneNumberUpdateDto { AccountId = _accountId };
    //    _mockAccountAccessor
    //        .Setup(accessor => accessor.GetAsync(It.IsAny<Guid>()))
    //        .ReturnsAsync(_account);
    //    _mockPhoneNumberAccessor
    //        .Setup(accessor => accessor.UpdateAsync(It.IsAny<Guid>(), It.IsAny<PhoneNumberUpdateDto>()))
    //        .ReturnsAsync(_phoneNumber);

    //    // Act
    //    var phoneNumber = await _phoneNumberManager.AssignPhoneNumberAsync(phoneNumberId, phoneNumberUpdateDto);

    //    // Assert
    //    _mockAccountAccessor.Verify(accessor => accessor.GetAsync(It.Is<Guid>(p => p.Equals(_accountId))), Times.Once);
    //}

    //[Test]
    //public async Task UpdatePhoneNumberAsync_With_Invalid_AccountId_Should_Return_Error()
    //{
    //    // Arrange
    //    AccountDto nullAccount = null;
    //    var phoneNumberId = Guid.NewGuid();
    //    var phoneNumberUpdateDto = new PhoneNumberUpdateDto { AccountId = Guid.NewGuid() };
    //    _mockAccountAccessor
    //        .Setup(accessor => accessor.GetAsync(It.IsAny<Guid>()))
    //        .ReturnsAsync(nullAccount);

    //    // Act
    //    var error = await _phoneNumberManager.AssignPhoneNumberAsync(phoneNumberId, phoneNumberUpdateDto) as CallError;

    //    // Assert
    //    Assert.That(error, Is.Not.Null);
    //    Assert.That(error.Code, Is.EqualTo(ErrorCodes.VOICEFLEX_0004));
    //}

    //[Test]
    //public async Task UpdatePhoneNumberAsync_With_Suspended_Account_Should_Return_Error()
    //{
    //    // Arrange
    //    var phoneNumberId = Guid.NewGuid();
    //    var phoneNumberUpdateDto = new PhoneNumberUpdateDto { AccountId = _suspendedAccountId };
    //    _mockAccountAccessor
    //        .Setup(accessor => accessor.GetAsync(It.IsAny<Guid>()))
    //        .ReturnsAsync(_suspendedAccount);

    //    // Act
    //    var error = await _phoneNumberManager.AssignPhoneNumberAsync(phoneNumberId, phoneNumberUpdateDto) as CallError;

    //    // Assert
    //    Assert.That(error, Is.Not.Null);
    //    Assert.That(error.Code, Is.EqualTo(ErrorCodes.VOICEFLEX_0003));
    //}

    //[Test]
    //public async Task UpdatePhoneNumberAsync_Should_Call_PhoneNumberAccessor_UpdateAsync_With_Correct_Parameters()
    //{
    //    // Arrange
    //    var phoneNumberId = Guid.NewGuid();
    //    var phoneNumberUpdateDto = new PhoneNumberUpdateDto();
    //    _mockPhoneNumberAccessor
    //        .Setup(accessor => accessor.UpdateAsync(It.IsAny<Guid>(), It.IsAny<PhoneNumberUpdateDto>()))
    //        .ReturnsAsync(_phoneNumber);

    //    // Act
    //    var actualPhoneNumber = await _phoneNumberManager.AssignPhoneNumberAsync(phoneNumberId, phoneNumberUpdateDto);

    //    // Assert
    //    _mockPhoneNumberAccessor.Verify(accessor => accessor.UpdateAsync(
    //        It.Is<Guid>(p => p.Equals(phoneNumberId)),
    //        It.Is<PhoneNumberUpdateDto>(p => p.Equals(phoneNumberUpdateDto))),
    //        Times.Once);
    //    Assert.That(actualPhoneNumber, Is.EqualTo(_phoneNumber));
    //}
}
