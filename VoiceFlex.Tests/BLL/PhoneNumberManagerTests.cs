using Moq;
using VoiceFlex.BLL;
using VoiceFlex.DAL;
using VoiceFlex.DTO;

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
        var expectedPhoneNumbers = new List<PhoneNumberDto>();

        _mockPhoneNumberAccessor
            .Setup(accessor => accessor.ListAsync())
            .ReturnsAsync(expectedPhoneNumbers);

        // Act
        var actualPhoneNumbers = await _phoneNumberManager.ListPhoneNumbersAsync();

        // Assert
        _mockPhoneNumberAccessor.Verify(accessor => accessor.ListAsync(), Times.Once);
        Assert.That(actualPhoneNumbers, Is.EqualTo(expectedPhoneNumbers));
    }
}
