using Moq;
using VoiceFlex.BLL;
using VoiceFlex.DAL;
using VoiceFlex.DTO;
using VoiceFlex.Models;

namespace VoiceFlex.Tests.BLL;

public class AccountManagerTests
{
    private Mock<IAccountValidator> _mockAccountValidator;
    private Mock<IAccountAccessor> _mockAccountAccessor;
    private AccountManager _accountManager;
    private AccountDto _accountDto;
    private Account _account;

    [SetUp]
    public void SetUp()
    {
        _mockAccountValidator = new Mock<IAccountValidator>();
        _mockAccountAccessor = new Mock<IAccountAccessor>();
        _accountManager = new AccountManager(_mockAccountValidator.Object, _mockAccountAccessor.Object);
        _accountDto = new AccountDto { Id = Guid.NewGuid() };
        _account = new Account();
    }

    [Test]
    public async Task CreateAccountAsync_Should_Call_Validator_And_Accessor_With_Correct_Parameters()
    {
        // Arrange
        _accountDto.Description = "test";

        _mockAccountValidator
            .Setup(v => v.DescriptionMustBeValid(It.IsAny<string>()))
            .Returns(_mockAccountValidator.Object);
        _mockAccountAccessor
            .Setup(accessor => accessor.CreateAsync(It.IsAny<AccountDto>()))
            .ReturnsAsync(_account);

        // Act
        var actualAccount = await _accountManager.CreateAccountAsync(_accountDto);

        // Assert
        _mockAccountValidator.Verify(v => v.DescriptionMustBeValid(It.Is<string>(p => p.Equals(_accountDto.Description))), Times.Once);
        _mockAccountAccessor.Verify(accessor => accessor.CreateAsync(It.Is<AccountDto>(p => p.Equals(_accountDto))), Times.Once);
        Assert.That(actualAccount, Is.EqualTo(_account));
    }

    [Test]
    public async Task GetAccountWithPhoneNumbersAsync_Should_Call_Validator_And_Accessor_With_Correct_Parameters()
    {
        // Arrange
        _mockAccountAccessor
            .Setup(accessor => accessor.GetAsync(It.IsAny<Guid>()))
            .ReturnsAsync(_accountDto);
        _mockAccountValidator
            .Setup(v => v.FoundInDatabase(It.IsAny<AccountDto>()))
            .Returns(_mockAccountValidator.Object);

        // Act
        var actualAccount = await _accountManager.GetAccountWithPhoneNumbersAsync(_accountDto.Id);

        // Assert
        _mockAccountAccessor.Verify(accessor => accessor.GetAsync(It.Is<Guid>(p => p.Equals(_accountDto.Id))), Times.Once);
        _mockAccountValidator.Verify(v => v.FoundInDatabase(It.Is<AccountDto>(p => p.Equals(_accountDto))), Times.Once);
        Assert.That(actualAccount, Is.EqualTo(_accountDto));
    }

    [TestCase(AccountStatus.Active)]
    [TestCase(AccountStatus.Suspended)]
    public async Task UpdateAccountAsync_Should_Call_Validator_And_Accessor_With_Correct_Parameters(AccountStatus status)
    {
        // Arrange
        var accountUpdateDto = new AccountUpdateDto { Status = status };
        _mockAccountAccessor
            .Setup(accessor => accessor.SetActiveAsync(It.IsAny<Guid>()))
            .ReturnsAsync(_account);
        _mockAccountAccessor
            .Setup(accessor => accessor.SetSuspendedAsync(It.IsAny<Guid>()))
            .ReturnsAsync(_account);
        _mockAccountValidator
            .Setup(v => v.FoundInDatabase(It.IsAny<Account>()))
            .Returns(_mockAccountValidator.Object);

        // Act
        var actualAccount = await _accountManager.UpdateAccountAsync(_accountDto.Id, accountUpdateDto);

        // Assert
        _mockAccountAccessor.Verify(accessor => accessor.SetActiveAsync(It.Is<Guid>(p => p.Equals(_accountDto.Id))),
            status == AccountStatus.Active ? Times.Once : Times.Never);
        _mockAccountAccessor.Verify(accessor => accessor.SetSuspendedAsync(It.Is<Guid>(p => p.Equals(_accountDto.Id))),
            status == AccountStatus.Active ? Times.Never : Times.Once);
        _mockAccountValidator.Verify(v => v.FoundInDatabase(It.Is<Account>(p => p.Equals(_account))), Times.Once);
        Assert.That(actualAccount, Is.EqualTo(_account));
    }
}
