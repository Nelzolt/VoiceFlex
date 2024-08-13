using Moq;
using VoiceFlex.BLL;
using VoiceFlex.DAL;
using VoiceFlex.Models;

namespace VoiceFlex.Tests.BLL;

public class PhoneNumberManagerTests
{
    private Mock<IPhoneNumberAccessor> _mockPhoneNumberAccessor;
    private PhoneNumberManager _phoneNumberManager;

    [SetUp]
    public void SetUp()
    {
        _mockPhoneNumberAccessor = new Mock<IPhoneNumberAccessor>();
        _phoneNumberManager = new PhoneNumberManager(_mockPhoneNumberAccessor.Object);
    }

    [Test]
    public async Task ListPhoneNumbersAsync_ShouldReturnListOfPhoneNumbers()
    {
        // Arrange
        var expectedPhoneNumbers = new List<PhoneNumber>
        {
            new() { Number = "1234567890" },
            new() { Number = "0987654321" }
        };

        _mockPhoneNumberAccessor
            .Setup(accessor => accessor.ListAsync())
            .ReturnsAsync(expectedPhoneNumbers);

        // Act
        var actualPhoneNumbers = await _phoneNumberManager.ListPhoneNumbersAsync();

        // Assert
        _mockPhoneNumberAccessor.Verify(accessor => accessor.ListAsync(), Times.Once);
        Assert.That(actualPhoneNumbers, Has.Count.EqualTo(expectedPhoneNumbers.Count));
        Assert.Multiple(() =>
        {
            Assert.That(actualPhoneNumbers[0].Number, Is.EqualTo(expectedPhoneNumbers[0].Number));
            Assert.That(actualPhoneNumbers[1].Number, Is.EqualTo(expectedPhoneNumbers[1].Number));
        });
    }

    [Test]
    public async Task ListPhoneNumbersAsync_ShouldWorkWithEmptyList()
    {
        // Arrange
        var expectedPhoneNumbers = new List<PhoneNumber>();

        _mockPhoneNumberAccessor
            .Setup(accessor => accessor.ListAsync())
            .ReturnsAsync(expectedPhoneNumbers);

        // Act
        var actualPhoneNumbers = await _phoneNumberManager.ListPhoneNumbersAsync();

        // Assert
        _mockPhoneNumberAccessor.Verify(accessor => accessor.ListAsync(), Times.Once);
        Assert.That(actualPhoneNumbers, Has.Count.EqualTo(0));
    }
}
