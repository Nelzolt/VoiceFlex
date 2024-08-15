using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net.Http.Json;
using VoiceFlex.BLL;
using VoiceFlex.DTO;
using VoiceFlex.Helpers;
using VoiceFlex.Models;
using VoiceFlex.Tests.TestHelpers;

namespace VoiceFlex.Tests.ApiEndpoints;

[TestFixture]
public class PhoneNumberApiEndpointsTests
{
    private WebApplicationFactory<Program> _factory;
    private HttpClient _httpClient;
    private Mock<IPhoneNumberManager> _mockPhoneNumberManager;
    private PhoneNumberDto _expectedPhoneNumber;
    private PhoneNumber _phoneNumber;

    [SetUp]
    public void SetUp()
    {
        _mockPhoneNumberManager = new Mock<IPhoneNumberManager>();
        _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.AddSingleton(_mockPhoneNumberManager.Object);
        });
        _httpClient = _factory.CreateClient();
        _expectedPhoneNumber = new PhoneNumberDto
        {
            Number = "0987654321"
        };
        _phoneNumber = new PhoneNumber
        {
            Id = Guid.NewGuid(),
            Number = "0987654321",
            AccountId = Guid.NewGuid()
        };
    }

    [Test]
    public async Task CreatePhoneNumberAsync_Should_Call_PhoneNumberManager_CreatePhoneNumberAsync_And_Return_NewPhoneNumber()
    {
        // Arrange
        _mockPhoneNumberManager.Setup(m => m.CreatePhoneNumberAsync(It.IsAny<PhoneNumberDto>())).ReturnsAsync(_expectedPhoneNumber);

        // Act
        var response = await _httpClient.PostAsJsonAsync("/api/phonenumbers", _expectedPhoneNumber);
        response.EnsureSuccessStatusCode();

        var actualPhoneNumber = await response.Content.ReadFromJsonAsync<PhoneNumberDto>();

        // Assert
        _mockPhoneNumberManager.Verify(m => m.CreatePhoneNumberAsync(It.Is<PhoneNumberDto>(
            p => p.Number.Equals(_expectedPhoneNumber.Number))),
            Times.Once);
        Assert.That(actualPhoneNumber.Number, Is.EqualTo(_expectedPhoneNumber.Number));
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase("123456789012")]
    public async Task CreatePhoneNumberAsync_Returns_400_Error_If_Number_Has_Wrong_Length(string number)
    {
        ErrorDto error = null;
        _expectedPhoneNumber.Number = number;

        // Arrange
        _mockPhoneNumberManager.Setup(m => m.CreatePhoneNumberAsync(It.IsAny<PhoneNumberDto>())).ReturnsAsync(_expectedPhoneNumber);

        // Act
        var response = await _httpClient.PostAsJsonAsync("/api/phonenumbers", _expectedPhoneNumber);
        var ex = Assert.Throws<HttpRequestException>(() => response.EnsureSuccessStatusCode());
        error = await response.Content.ReadFromJsonAsync<ErrorDto>();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(error.Code, Is.EqualTo(ErrorCodes.VOICEFLEX_0002.ToString()));
            Assert.That(error.Message, Is.EqualTo("The phone number must be between 1 and 11 characters."));
        });
    }

    [Test]
    public async Task UpdatePhoneNumberAsync_Should_Call_PhoneNumberManager_UpdatePhoneNumberAsync_And_Return_UpdatedPhoneNumber()
    {
        // Arrange
        var phoneNumberUpdateDto = new PhoneNumberUpdateDto();
        _mockPhoneNumberManager.Setup(m => m.UpdatePhoneNumberAsync(It.IsAny<Guid>(), It.IsAny<PhoneNumberUpdateDto>())).ReturnsAsync(_phoneNumber);

        // Act
        var response = await _httpClient.PatchAsJsonAsync($"/api/phoneNumbers/{_phoneNumber.Id}", phoneNumberUpdateDto);
        response.EnsureSuccessStatusCode();

        var actualPhoneNumber = await response.Content.ReadFromJsonAsync<PhoneNumber>();

        // Assert
        _mockPhoneNumberManager.Verify(m => m.UpdatePhoneNumberAsync(
            It.Is<Guid>(id => id.Equals(_phoneNumber.Id)),
            It.Is<PhoneNumberUpdateDto>(p => p.AccountId.Equals(phoneNumberUpdateDto.AccountId))),
            Times.Once);
        Assert.Multiple(() =>
        {
            Assert.That(actualPhoneNumber.Id, Is.EqualTo(_phoneNumber.Id));
            Assert.That(actualPhoneNumber.Number, Is.EqualTo(_phoneNumber.Number));
            Assert.That(actualPhoneNumber.AccountId, Is.EqualTo(_phoneNumber.AccountId));
        });
    }

    [Test]
    public async Task DeletePhoneNumberAsync_Should_Call_PhoneNumberManager_DeletePhoneNumberAsync()
    {
        // Arrange
        var phoneNumberId = Guid.NewGuid();

        // Act
        var response = await _httpClient.DeleteAsync($"/api/phonenumbers/{phoneNumberId}");
        response.EnsureSuccessStatusCode();

        // Assert
        _mockPhoneNumberManager.Verify(m => m.DeletePhoneNumberAsync(It.Is<Guid>(p => p.Equals(phoneNumberId))), Times.Once);
    }

    [TearDown]
    public void TearDown()
    {
        _factory.Dispose();
        _httpClient.Dispose();
    }
}
