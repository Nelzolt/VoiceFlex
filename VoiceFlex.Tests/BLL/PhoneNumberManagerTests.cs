using Moq;
using VoiceFlex.BLL;
using VoiceFlex.DAL;
using VoiceFlex.DTO;
using VoiceFlex.Models;

namespace VoiceFlex.Tests.BLL;

[TestFixture]
public class PhoneNumberManagerTests
{
    private Mock<IPhoneNumberValidator> _mockPhoneNumberValidator;
    private Mock<IPhoneNumberAccessor> _mockPhoneNumberAccessor;
    private Mock<IAccountAccessor> _mockAccountAccessor;
    private PhoneNumberManager _phoneNumberManager;
    private PhoneNumberDto _phoneNumberDto;
    private PhoneNumberUpdateDto _phoneNumberUpdateDto;
    private PhoneNumber _phoneNumber;
    private Guid _accountId;
    private AccountDto _accountDto;
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
        _phoneNumberUpdateDto = new PhoneNumberUpdateDto();
        _phoneNumber = new PhoneNumber
        {
            Id = _phoneNumberId,
            Number = "1234"
        };
        _accountDto = new AccountDto
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
            .Setup(a => a.GetByNumberAsync(It.IsAny<string>()))
            .ReturnsAsync(_phoneNumber);
        _mockPhoneNumberValidator
            .Setup(v => v.NumberMustBeNew(It.IsAny<PhoneNumber>()))
            .Returns(_mockPhoneNumberValidator.Object);
        _mockPhoneNumberValidator
            .Setup(v => v.NumberMustBeValid(It.IsAny<string>()))
            .Returns(_mockPhoneNumberValidator.Object);
        _mockPhoneNumberAccessor
            .Setup(a => a.CreateAsync(It.IsAny<PhoneNumberDto>()))
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

    [TestCaseSource(nameof(GetPhoneNumberUpdateDtos))]
    public async Task AssignUnassignPhoneNumberAsync_Assign_Should_Call_Accessors_And_Validators_With_Correct_Parameters(PhoneNumberUpdateDto _phoneNumberUpdateDto)
    {
        // Arrange
        _accountDto.Id = _phoneNumberUpdateDto.AccountId ?? Guid.Empty;
        _mockPhoneNumberAccessor
            .Setup(a => a.GetAsync(It.IsAny<Guid>()))
            .ReturnsAsync(_phoneNumber);
        _mockAccountAccessor
            .Setup(a => a.GetAsync(It.IsAny<Guid>()))
            .ReturnsAsync(_accountDto);

        _mockPhoneNumberValidator
            .Setup(v => v.FoundInDatabase(It.IsAny<PhoneNumber>()))
            .Returns(_mockPhoneNumberValidator.Object);
        _mockPhoneNumberValidator
            .Setup(v => v.AccountFoundInDatabase(It.IsAny<AccountDto>()))
            .Returns(_mockPhoneNumberValidator.Object);
        _mockPhoneNumberValidator
            .Setup(v => v.AccountMustBeActive(It.IsAny<AccountStatus?>()))
            .Returns(_mockPhoneNumberValidator.Object);
        _mockPhoneNumberValidator
            .Setup(v => v.PhoneNumberMustBeUnassigned(It.IsAny<Guid?>()))
            .Returns(_mockPhoneNumberValidator.Object);

        _mockPhoneNumberAccessor
            .Setup(a => a.AssignAsync(It.IsAny<PhoneNumber>(), It.IsAny<Guid?>()))
            .ReturnsAsync(_phoneNumber);
        _mockPhoneNumberAccessor
            .Setup(a => a.UnassignAsync(It.IsAny<PhoneNumber>()))
            .ReturnsAsync(_phoneNumber);

        // Act
        var assignedPhoneNumber = await _phoneNumberManager.AssignUnassignPhoneNumberAsync(_phoneNumberId, _phoneNumberUpdateDto);

        // Assert
        _mockPhoneNumberAccessor.Verify(a => a.GetAsync(It.Is<Guid>(p => p.Equals(_phoneNumberId))), Times.Once);
        _mockPhoneNumberValidator.Verify(a => a.FoundInDatabase(It.Is<PhoneNumber>(p => p.Equals(_phoneNumber))), Times.Once);
        Assert.That(assignedPhoneNumber, Is.EqualTo(_phoneNumber));

        if (_phoneNumberUpdateDto.AccountId is null || _phoneNumberUpdateDto.AccountId == Guid.Empty)
        {
            _mockPhoneNumberAccessor.Verify(a => a.UnassignAsync(It.Is<PhoneNumber>(p => p.Equals(_phoneNumber))), Times.Once);
        }
        else
        {
            _mockAccountAccessor.Verify(a => a.GetAsync(It.Is<Guid>(p => p.Equals(_phoneNumberUpdateDto.AccountId))), Times.Once);
            _mockPhoneNumberValidator.Verify(a => a.AccountFoundInDatabase(It.Is<AccountDto>(p => p.Equals(_accountDto))), Times.Once);
            _mockPhoneNumberValidator.Verify(a => a.AccountMustBeActive(It.Is<AccountStatus>(p => p.Equals(_accountDto.Status))), Times.Once);
            _mockPhoneNumberValidator.Verify(a => a.PhoneNumberMustBeUnassigned(It.Is<Guid?>(p => p.Equals(_phoneNumber.AccountId))), Times.Once);
            _mockPhoneNumberAccessor.Verify(a => a.AssignAsync(
                It.Is<PhoneNumber>(p => p.Equals(_phoneNumber)),
                It.Is<Guid>(p => p.Equals(_phoneNumberUpdateDto.AccountId))), Times.Once);
        }
    }

    public static IEnumerable<PhoneNumberUpdateDto> GetPhoneNumberUpdateDtos()
    {
        yield return new PhoneNumberUpdateDto { AccountId = Guid.NewGuid() };
        yield return new PhoneNumberUpdateDto { AccountId = Guid.Empty };
        yield return new PhoneNumberUpdateDto { AccountId = null };
    }

    [Test]
    public async Task DeletePhoneNumberAsync_Should_Call_Accessor_And_Validator_With_Correct_Parameters()
    {
        // Arrange
        var phoneNumberId = Guid.NewGuid();
        _mockPhoneNumberAccessor
            .Setup(a => a.DeleteAsync(It.IsAny<Guid>()))
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
}
