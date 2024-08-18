using Microsoft.EntityFrameworkCore;
using VoiceFlex.DAL;
using VoiceFlex.Data;
using VoiceFlex.DTO;
using VoiceFlex.Models;

namespace VoiceFlex.Tests.DAL;

public class PhoneNumberAccessorTests
{
    private ApplicationDbContext _dbContext;
    private PhoneNumberAccessor _phoneNumberAccessor;
    private PhoneNumber _phoneNumber;
    private PhoneNumberDto _newPhoneNumber;
    private PhoneNumber _phoneNumberToDelete;
    private PhoneNumber _assignedPhoneNumber;
    private Guid _accountId;
    private Guid _newAccountId;
    private Guid _suspendedAccountId;
    private Account _account;
    private Account _suspendedAccount;

    [SetUp]
    public void SetUp()
    {
        _accountId = Guid.NewGuid();
        _newAccountId = Guid.NewGuid();
        _suspendedAccountId = Guid.NewGuid();
        _account = new Account
        {
            Id = _accountId,
            Status = AccountStatus.Active
        };
        _suspendedAccount = new Account
        {
            Id = _suspendedAccountId,
            Status = AccountStatus.Suspended
        };
        _phoneNumber = new PhoneNumber
        {
            Number = "0555444"
        };
        _assignedPhoneNumber = new PhoneNumber
        {
            Number = "055665444",
            AccountId = _accountId
        };
        _newPhoneNumber = new PhoneNumberDto
        {
            Number = "123456789"
        };
        _phoneNumberToDelete = new PhoneNumber
        {
            Number = "321"
        };

        _dbContext = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase").Options);
        _dbContext.VOICEFLEX_PhoneNumbers.Add(_phoneNumber);
        _dbContext.VOICEFLEX_PhoneNumbers.Add(_phoneNumberToDelete);
        _dbContext.VOICEFLEX_PhoneNumbers.Add(_assignedPhoneNumber);
        _dbContext.VOICEFLEX_Accounts.Add(_account);
        _dbContext.VOICEFLEX_Accounts.Add(_suspendedAccount);
        _dbContext.SaveChanges();

        _phoneNumberAccessor = new PhoneNumberAccessor(_dbContext);
    }

    [Test]
    public async Task CreateAsync_Should_Add_PhoneNumber_To_Db_And_Return_PhoneNumber_With_Id()
    {
        // Act
        var actualPhoneNumber = await _phoneNumberAccessor.CreateAsync(_newPhoneNumber) as PhoneNumber;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(actualPhoneNumber.Number, Is.EqualTo(_newPhoneNumber.Number));
            Assert.That(actualPhoneNumber.Id, Is.Not.EqualTo(Guid.Empty));
        });
        var createdPhoneNumber = await _dbContext.VOICEFLEX_PhoneNumbers.FindAsync(actualPhoneNumber.Id);
        Assert.That(createdPhoneNumber, Is.Not.Null);
    }

    [Test]
    public async Task UpdateAsync_Should_Assign_Unassign_PhoneNumber_In_Db_And_Return_Updated_PhoneNumber()
    {
        // Arrange
        var phoneNumberUpdateDto = new PhoneNumberUpdateDto { AccountId = _accountId };

        // Act
        var updatedPhoneNumber = await _phoneNumberAccessor.AssignAsync(_phoneNumber, _accountId);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(updatedPhoneNumber.Id, Is.EqualTo(_phoneNumber.Id));
            Assert.That(updatedPhoneNumber.Number, Is.EqualTo(_phoneNumber.Number));
            Assert.That(updatedPhoneNumber.AccountId, Is.EqualTo(phoneNumberUpdateDto.AccountId));
        });

        // Act
        updatedPhoneNumber = await _phoneNumberAccessor.UnassignAsync(_phoneNumber);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(updatedPhoneNumber.Id, Is.EqualTo(_phoneNumber.Id));
            Assert.That(updatedPhoneNumber.Number, Is.EqualTo(_phoneNumber.Number));
            Assert.That(updatedPhoneNumber.AccountId, Is.Null);
        });
    }

    [Test]
    public async Task DeleteAsync_Should_Delete_PhoneNumber_From_Db()
    {
        // Act
        await _phoneNumberAccessor.DeleteAsync(_phoneNumberToDelete.Id);

        // Assert
        var deletedPhoneNumber = await _dbContext.VOICEFLEX_PhoneNumbers.FindAsync(_phoneNumberToDelete.Id);
        Assert.That(deletedPhoneNumber, Is.Null);
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
}
