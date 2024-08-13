using Microsoft.EntityFrameworkCore;
using VoiceFlex.DAL;
using VoiceFlex.Data;
using VoiceFlex.Models;

namespace VoiceFlex.Tests.DAL;

public class PhoneNumberAccessorTests
{
    private ApplicationDbContext _dbContext;
    private PhoneNumberAccessor _phoneNumberAccessor;
    private List<PhoneNumber> _expectedPhoneNumbers;

    [SetUp]
    public void SetUp()
    {
        _expectedPhoneNumbers = new List<PhoneNumber>
        {
            new() { Id = Guid.NewGuid(), Number = "1234567890", AccountId = null },
            new() { Id = Guid.NewGuid(), Number = "0987654321", AccountId = Guid.NewGuid() }
        };

        _dbContext = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase").Options);
        _dbContext.VOICEFLEX_PhoneNumbers.AddRange(_expectedPhoneNumbers);
        _dbContext.SaveChanges();

        _phoneNumberAccessor = new PhoneNumberAccessor(_dbContext);
    }

    [Test]
    public async Task ListAsync_ShouldReturnPhoneNumberDtoList()
    {
        // Act
        var actualPhoneNumbers = await _phoneNumberAccessor.ListAsync();

        // Assert
        Assert.That(actualPhoneNumbers, Has.Count.EqualTo(_expectedPhoneNumbers.Count));
        Assert.Multiple(() =>
        {
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
