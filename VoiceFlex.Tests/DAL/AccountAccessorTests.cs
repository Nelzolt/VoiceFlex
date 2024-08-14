using Microsoft.EntityFrameworkCore;
using VoiceFlex.DAL;
using VoiceFlex.Data;
using VoiceFlex.Models;

namespace VoiceFlex.Tests.DAL;

public class AccountAccessorTests
{
    private ApplicationDbContext _dbContext;
    private AccountAccessor _accountAccessor;
    private Account _expectedAccount;
    private List<PhoneNumber> _expectedPhoneNumbers;
    private Guid _accountId;

    [SetUp]
    public void SetUp()
    {
        _accountId = Guid.NewGuid();
        _expectedPhoneNumbers = new List<PhoneNumber>
            {
                new() { Id = Guid.NewGuid(), Number = "1234567890", AccountId = _accountId },
                new() { Id = Guid.NewGuid(), Number = "0987654321", AccountId = _accountId }
            };
        _expectedAccount = new Account
        {
            Id = _accountId,
            PhoneNumbers = _expectedPhoneNumbers
        };

        _dbContext = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase").Options);
        _dbContext.VOICEFLEX_Accounts.Add(_expectedAccount);
        _dbContext.SaveChanges();

        _accountAccessor = new AccountAccessor(_dbContext);
    }

    [Test]
    public async Task GetAsync_Should_Return_Account_With_PhoneNumbers()
    {
        // Act
        var actualAccount = await _accountAccessor.GetAsync(_accountId);
        var actualPhoneNumbers = actualAccount.PhoneNumbers;

        // Assert - Mind that the result list must be sorted by number
        Assert.That(actualPhoneNumbers, Has.Count.EqualTo(_expectedAccount.PhoneNumbers.Count));
        Assert.Multiple(() =>
        {
            Assert.That(actualAccount.Id, Is.EqualTo(_expectedAccount.Id));
            Assert.That(actualPhoneNumbers[0].Id, Is.EqualTo(_expectedPhoneNumbers[1].Id));
            Assert.That(actualPhoneNumbers[0].Number, Is.EqualTo(_expectedPhoneNumbers[1].Number));
            Assert.That(actualPhoneNumbers[0].AccountId, Is.EqualTo(_expectedPhoneNumbers[1].AccountId));
            Assert.That(actualPhoneNumbers[1].Id, Is.EqualTo(_expectedPhoneNumbers[0].Id));
            Assert.That(actualPhoneNumbers[1].Number, Is.EqualTo(_expectedPhoneNumbers[0].Number));
            Assert.That(actualPhoneNumbers[1].AccountId, Is.EqualTo(_expectedPhoneNumbers[0].AccountId));
        });
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
}
