using Microsoft.EntityFrameworkCore;
using VoiceFlex.DAL;
using VoiceFlex.Data;
using VoiceFlex.DTO;

namespace VoiceFlex.Tests.DAL;

public class PhoneNumberAccessorTests
{
    private ApplicationDbContext _dbContext;
    private PhoneNumberAccessor _phoneNumberAccessor;
    private PhoneNumberDto _newPhoneNumber;

    [SetUp]
    public void SetUp()
    {
        _newPhoneNumber = new PhoneNumberDto
        {
            Number = "123456789"
        };

        _dbContext = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase").Options);
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
        var createdPhoneNumber = await _dbContext.VOICEFLEX_PhoneNumbers
            .Where(a => a.Id.Equals(actualPhoneNumber.Id))
            .FirstOrDefaultAsync();
        Assert.That(createdPhoneNumber, Is.Not.Null);
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
}
