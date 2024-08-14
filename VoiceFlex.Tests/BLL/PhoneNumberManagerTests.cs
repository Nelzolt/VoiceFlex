using Moq;
using VoiceFlex.BLL;
using VoiceFlex.DAL;
using VoiceFlex.DTO;
using VoiceFlex.Models;

namespace VoiceFlex.Tests.BLL;

public class PhoneNumberManagerTests
{
    private Mock<IPhoneNumberAccessor> _mockPhoneNumberAccessor;
    private PhoneNumberManager _phoneNumberManager;
    private PhoneNumberDto _expectedPhoneNumber;
    private PhoneNumber _phoneNumber;

    [SetUp]
    public void SetUp()
    {
        _mockPhoneNumberAccessor = new Mock<IPhoneNumberAccessor>();
        _phoneNumberManager = new PhoneNumberManager(_mockPhoneNumberAccessor.Object);
        _expectedPhoneNumber = new PhoneNumberDto();
    }

    [Test]
    public async Task CreatePhoneNumberAsync_Should_Call_PhoneNumberAccessor_CreateAsync_With_Correct_Parameters()
    {
        // Arrange
        _mockPhoneNumberAccessor
            .Setup(accessor => accessor.CreateAsync(It.IsAny<PhoneNumberDto>()))
            .ReturnsAsync(_expectedPhoneNumber);

        // Act
        var actualPhoneNumber = await _phoneNumberManager.CreatePhoneNumberAsync(_expectedPhoneNumber);

        // Assert
        _mockPhoneNumberAccessor.Verify(accessor => accessor.CreateAsync(It.Is<PhoneNumberDto>(p => p.Equals(_expectedPhoneNumber))), Times.Once);
        Assert.That(actualPhoneNumber, Is.EqualTo(_expectedPhoneNumber));
    }

    [Test]
    public async Task UpdatePhoneNumberAsync_Should_Call_PhoneNumberAccessor_UpdateAsync_With_Correct_Parameters()
    {
        // Arrange
        var phoneNumberId = Guid.NewGuid();
        var phoneNumberUpdateDto = new PhoneNumberUpdateDto();
        _mockPhoneNumberAccessor
            .Setup(accessor => accessor.UpdateAsync(It.IsAny<Guid>(), It.IsAny<PhoneNumberUpdateDto>()))
            .ReturnsAsync(_phoneNumber);

        // Act
        var actualPhoneNumber = await _phoneNumberManager.UpdatePhoneNumberAsync(phoneNumberId, phoneNumberUpdateDto);

        // Assert
        _mockPhoneNumberAccessor.Verify(accessor => accessor.UpdateAsync(
            It.Is<Guid>(p => p.Equals(phoneNumberId)),
            It.Is<PhoneNumberUpdateDto>(p => p.Equals(phoneNumberUpdateDto))),
            Times.Once);
        Assert.That(actualPhoneNumber, Is.EqualTo(_phoneNumber));
    }

    [Test]
    public async Task DeletePhoneNumberAsync_Should_Call_PhoneNumberAccessor_DeleteAsync_With_Correct_Parameters()
    {
        // Arrange
        var phoneNumberId = Guid.NewGuid();

        // Act
        await _phoneNumberManager.DeletePhoneNumberAsync(phoneNumberId);

        // Assert
        _mockPhoneNumberAccessor.Verify(accessor => accessor.DeleteAsync(It.Is<Guid>(p => p.Equals(phoneNumberId))), Times.Once);
    }
}
