using Microsoft.EntityFrameworkCore;
using VoiceFlex.DAL;
using VoiceFlex.Data;
using VoiceFlex.DTO;
using VoiceFlex.Models;

namespace VoiceFlex.Tests.DAL;

public class AccountAccessorTests
{
    private ApplicationDbContext _dbContext;
    private AccountAccessor _accountAccessor;
    private Account _expectedAccount;
    private AccountDto _newAccount;
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
        _newAccount = new AccountDto
        {
            Description = "test",
            Status = AccountStatus.Active
        };

        _dbContext = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase").Options);
        _dbContext.VOICEFLEX_Accounts.Add(_expectedAccount);
        _dbContext.SaveChanges();

        _accountAccessor = new AccountAccessor(_dbContext);
    }

    [Test]
    public async Task CreateAsync_Should_Add_Account_To_Db_And_Return_Account_With_Id()
    {
        // Act
        var actualAccount = await _accountAccessor.CreateAsync(_newAccount);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(actualAccount.Description, Is.EqualTo(_newAccount.Description));
            Assert.That(actualAccount.Status, Is.EqualTo(_newAccount.Status));
            Assert.That(actualAccount.Id, Is.Not.EqualTo(Guid.Empty));
        });
        var createdAccount = await _dbContext.VOICEFLEX_Accounts
            .Where(a => a.Id.Equals(actualAccount.Id))
            .FirstOrDefaultAsync();
        Assert.That(createdAccount, Is.Not.Null);
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
