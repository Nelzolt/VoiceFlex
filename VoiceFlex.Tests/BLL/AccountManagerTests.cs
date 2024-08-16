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
    private AccountDto _expectedAccount;
    private Account _account;

    [SetUp]
    public void SetUp()
    {
        _mockAccountValidator = new Mock<IAccountValidator>();
        _mockAccountAccessor = new Mock<IAccountAccessor>();
        _accountManager = new AccountManager(_mockAccountValidator.Object, _mockAccountAccessor.Object);
        _expectedAccount = new AccountDto();
    }

    [Test]
    public async Task CreateAccountAsync_Should_Call_AccountAccessor_CreateAsync_With_Correct_Parameters()
    {
        // Arrange
        _mockAccountAccessor
            .Setup(accessor => accessor.CreateAsync(It.IsAny<AccountDto>()))
            .ReturnsAsync(_account);

        // Act
        var actualAccount = await _accountManager.CreateAccountAsync(_expectedAccount);

        // Assert
        _mockAccountAccessor.Verify(accessor => accessor.CreateAsync(It.Is<AccountDto>(p => p.Equals(_expectedAccount))), Times.Once);
        Assert.That(actualAccount, Is.EqualTo(_account));
    }

    [Test]
    public async Task CreateAccountAsync_Should_Call_AccountValidator_Error_With_Correct_Parameters()
    {
        // Arrange
        _mockAccountAccessor
            .Setup(accessor => accessor.CreateAsync(It.IsAny<AccountDto>()))
            .ReturnsAsync(_account);

        // Act
        var actualAccount = await _accountManager.CreateAccountAsync(_expectedAccount);

        // Assert
        _mockAccountAccessor.Verify(accessor => accessor.CreateAsync(It.Is<AccountDto>(p => p.Equals(_expectedAccount))), Times.Once);
        Assert.That(actualAccount, Is.EqualTo(_account));
    }

    [Test]
    public async Task GetAccountWithPhoneNumbersAsync_Should_Call_AccountAccessor_GetAsync_With_Correct_Parameters()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        _mockAccountAccessor
            .Setup(accessor => accessor.GetAsync(It.IsAny<Guid>()))
            .ReturnsAsync(_expectedAccount);

        // Act
        var actualAccount = await _accountManager.GetAccountWithPhoneNumbersAsync(accountId);

        // Assert
        _mockAccountAccessor.Verify(accessor => accessor.GetAsync(It.Is<Guid>(p => p.Equals(accountId))), Times.Once);
        Assert.That(actualAccount, Is.EqualTo(_expectedAccount));
    }

    [TestCase(AccountStatus.Active)]
    [TestCase(AccountStatus.Suspended)]
    public async Task UpdateAccountAsync_Should_Call_AccountAccessor_SetActiveAsync_Or_SetSuspendedAsync_With_Correct_Parameters(AccountStatus accountStatus)
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var accountUpdateDto = new AccountUpdateDto { Status = accountStatus };
        _mockAccountAccessor
            .Setup(accessor => accessor.SetActiveAsync(It.IsAny<Guid>()))
            .ReturnsAsync(_account);

        // Act
        var actualAccount = await _accountManager.UpdateAccountAsync(accountId, accountUpdateDto);

        // Assert
        if (accountStatus == AccountStatus.Active)
        {
            _mockAccountAccessor.Verify(accessor => accessor.SetActiveAsync(It.Is<Guid>(p => p.Equals(accountId))), Times.Once);
        }
        else
        {
            _mockAccountAccessor.Verify(accessor => accessor.SetSuspendedAsync(It.Is<Guid>(p => p.Equals(accountId))), Times.Once);
        }
        Assert.That(actualAccount, Is.EqualTo(_account));
    }
}
