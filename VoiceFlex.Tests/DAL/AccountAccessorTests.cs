using Microsoft.EntityFrameworkCore;
using VoiceFlex.BLL;
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
    private Account _suspendedAccount;
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
            Description = "account",
            Status = AccountStatus.Active,
            PhoneNumbers = _expectedPhoneNumbers
        };
        _suspendedAccount = new Account
        {
            Id = Guid.NewGuid(),
            Description = "suspended",
            Status = AccountStatus.Suspended
        };
        _newAccount = new AccountDto
        {
            Description = "test",
            Status = AccountStatus.Active
        };

        _dbContext = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase").Options);
        _dbContext.VOICEFLEX_Accounts.Add(_expectedAccount);
        _dbContext.VOICEFLEX_Accounts.Add(_suspendedAccount);
        _dbContext.SaveChanges();

        _accountAccessor = new AccountAccessor(_dbContext);
    }

    [Test]
    public async Task CreateAsync_Should_Add_Account_To_Db_And_Return_Account_With_Id()
    {
        // Act
        var actualAccount = await _accountAccessor.CreateAsync(_newAccount) as Account;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(actualAccount.Description, Is.EqualTo(_newAccount.Description));
            Assert.That(actualAccount.Status, Is.EqualTo(_newAccount.Status));
            Assert.That(actualAccount.Id, Is.Not.EqualTo(Guid.Empty));
        });
        var createdAccount = await _dbContext.VOICEFLEX_Accounts.FindAsync(actualAccount.Id);
        Assert.That(createdAccount, Is.Not.Null);
    }

    [Test]
    public async Task GetAsync_Should_Return_Account_With_PhoneNumbers()
    {
        // Act
        var actualAccount = await _accountAccessor.GetAsync(_accountId) as AccountDto;
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

    [Test]
    public async Task GetAsync_Should_Return_Error_If_Account_With_This_Id_Is_Not_Found()
    {
        // Act
        var error = await _accountAccessor.GetAsync(Guid.NewGuid()) as CallError;

        // Assert
        Assert.That(error, Is.Not.Null);
        Assert.That(error.Code, Is.EqualTo(ErrorCodes.VOICEFLEX_0000));
    }

    [Test]
    public async Task SetActiveAsync_Should_Update_Account_In_Db_And_Return_Updated_Account()
    {
        // Act
        var updatedAccount = await _accountAccessor.SetActiveAsync(_suspendedAccount.Id) as Account;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(updatedAccount.Id, Is.EqualTo(_suspendedAccount.Id));
            Assert.That(updatedAccount.Status, Is.EqualTo(AccountStatus.Active));
            Assert.That(updatedAccount.Description, Is.EqualTo(_suspendedAccount.Description));
        });
    }

    [Test]
    public async Task SetActiveAsync_Should_Return_Error_If_Account_With_This_Id_Is_Not_Found()
    {
        // Act
        var error = await _accountAccessor.SetActiveAsync(Guid.NewGuid()) as CallError;

        // Assert
        Assert.That(error, Is.Not.Null);
        Assert.That(error.Code, Is.EqualTo(ErrorCodes.VOICEFLEX_0000));
    }

    [Test]
    public async Task SetSuspenedAsync_Should_Update_Account_In_Db_And_Unassign_PhoneNumbers_And_Return_Updated_Account()
    {
        // Act
        var updatedAccount = await _accountAccessor.SetSuspendedAsync(_expectedAccount.Id) as Account;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(updatedAccount.Id, Is.EqualTo(_expectedAccount.Id));
            Assert.That(updatedAccount.Status, Is.EqualTo(AccountStatus.Suspended));
            Assert.That(updatedAccount.Description, Is.EqualTo(_expectedAccount.Description));
            Assert.That(updatedAccount.PhoneNumbers.Count, Is.EqualTo(0));
        });
    }

    [Test]
    public async Task SetSuspenedAsync_Should_Return_Error_If_Account_With_This_Id_Is_Not_Found()
    {
        // Act
        var error = await _accountAccessor.SetSuspendedAsync(Guid.NewGuid()) as CallError;

        // Assert
        Assert.That(error, Is.Not.Null);
        Assert.That(error.Code, Is.EqualTo(ErrorCodes.VOICEFLEX_0000));
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
}
