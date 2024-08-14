using Moq;
using VoiceFlex.BLL;
using VoiceFlex.DAL;
using VoiceFlex.DTO;

namespace VoiceFlex.Tests.BLL;

public class AccountManagerTests
{
    private Mock<IAccountAccessor> _mockAccountAccessor;
    private AccountManager _accountManager;
    private AccountDto _expectedAccount;

    [SetUp]
    public void SetUp()
    {
        _mockAccountAccessor = new Mock<IAccountAccessor>();
        _accountManager = new AccountManager(_mockAccountAccessor.Object);
        _expectedAccount = new AccountDto();
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
}
