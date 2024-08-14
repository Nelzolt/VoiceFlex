using Moq;
using VoiceFlex.BLL;
using VoiceFlex.DAL;
using VoiceFlex.DTO;

namespace VoiceFlex.Tests.BLL;

public class PhoneNumberManagerTests
{
    private Mock<IPhoneNumberAccessor> _mockPhoneNumberAccessor;
    private PhoneNumberManager _phoneNumberManager;
    private List<PhoneNumberDto> _expectedPhoneNumbers;

    [SetUp]
    public void SetUp()
    {
        _mockPhoneNumberAccessor = new Mock<IPhoneNumberAccessor>();
        _phoneNumberManager = new PhoneNumberManager(_mockPhoneNumberAccessor.Object);
        _expectedPhoneNumbers = new List<PhoneNumberDto>();
    }

    [Test]
    public async Task ListPhoneNumbersAsync_ShouldReturnListOfPhoneNumbers()
    {
        // Arrange
        _mockPhoneNumberAccessor
            .Setup(accessor => accessor.ListAsync())
            .ReturnsAsync(_expectedPhoneNumbers);

        // Act
        var actualPhoneNumbers = await _phoneNumberManager.ListPhoneNumbersAsync();

        // Assert
        _mockPhoneNumberAccessor.Verify(accessor => accessor.ListAsync(), Times.Once);
        Assert.That(actualPhoneNumbers, Is.EqualTo(_expectedPhoneNumbers));
    }
}
