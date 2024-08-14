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
    private PhoneNumberDto _newPhoneNumber;
    private PhoneNumber _phoneNumberToDelete;

    [SetUp]
    public void SetUp()
    {
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
        _dbContext.VOICEFLEX_PhoneNumbers.Add(_phoneNumberToDelete);
        _dbContext.SaveChanges();

        _phoneNumberAccessor = new PhoneNumberAccessor(_dbContext);
    }

    [Test]
    public async Task CreateAsync_Should_Add_PhoneNumber_To_Db_And_Return_PhoneNumber_With_Id()
    {
        // Act
        var actualPhoneNumber = await _phoneNumberAccessor.CreateAsync(_newPhoneNumber);

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
