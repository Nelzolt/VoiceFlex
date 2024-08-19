using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net.Http.Json;
using VoiceFlex.BLL;
using VoiceFlex.DTO;
using VoiceFlex.Models;
using VoiceFlex.Tests.TestHelpers;

namespace VoiceFlex.Tests.ApiEndpoints;

[TestFixture]
public class PhoneNumberApiEndpointsTests
{
    private WebApplicationFactory<Program> _factory;
    private HttpClient _httpClient;
    private Mock<IPhoneNumberValidator> _mockPhoneNumberValidator;
    private Mock<IPhoneNumberManager> _mockPhoneNumberManager;
    private PhoneNumberDto _expectedPhoneNumber;
    private PhoneNumber _phoneNumber;
    private CallError _callError;

    [SetUp]
    public void SetUp()
    {
        _mockPhoneNumberValidator = new Mock<IPhoneNumberValidator>();
        _mockPhoneNumberManager = new Mock<IPhoneNumberManager>();
        _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.AddSingleton(_mockPhoneNumberValidator.Object);
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
        _callError = new CallError(ErrorCodes.VOICEFLEX_0000);
    }

    [Test]
    public async Task CreatePhoneNumberAsync_Should_Call_PhoneNumberManager_CreatePhoneNumberAsync_And_Return_NewPhoneNumber()
    {
        // Arrange
        _mockPhoneNumberManager.Setup(m => m.CreatePhoneNumberAsync(It.IsAny<PhoneNumberDto>())).ReturnsAsync(_phoneNumber);

        // Act
        var response = await _httpClient.PostAsJsonAsync("/api/phonenumbers", _phoneNumber);
        response.EnsureSuccessStatusCode();

        var actualPhoneNumber = await response.Content.ReadFromJsonAsync<PhoneNumberDto>();

        // Assert
        _mockPhoneNumberManager.Verify(m => m.CreatePhoneNumberAsync(It.Is<PhoneNumberDto>(
            p => p.Number.Equals(_phoneNumber.Number))),
            Times.Once);
        Assert.That(actualPhoneNumber.Number, Is.EqualTo(_phoneNumber.Number));
    }

    [Test]
    public async Task CreatePhoneNumberAsync_Should_Return_Error_From_Manager()
    {
        // Arrange
        var error = new CallError(ErrorCodes.VOICEFLEX_0001);
        _mockPhoneNumberManager.Setup(m => m.CreatePhoneNumberAsync(It.IsAny<PhoneNumberDto>())).ReturnsAsync(error);

        // Act
        var response = await _httpClient.PostAsJsonAsync("/api/phonenumbers", _expectedPhoneNumber);
        var ex = Assert.Throws<HttpRequestException>(() => response.EnsureSuccessStatusCode());
        var result = await response.Content.ReadFromJsonAsync<ErrorDto>();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Code, Is.EqualTo(ErrorCodes.VOICEFLEX_0001.ToString()));
            Assert.That(result.Message, Is.EqualTo("The number must have at least 1 and not more than 11 characters."));
        });
    }

    [Test]
    public async Task UpdatePhoneNumberAsync_Should_Call_PhoneNumberManager_UpdatePhoneNumberAsync_And_Return_UpdatedPhoneNumber()
    {
        // Arrange
        var phoneNumberUpdateDto = new PhoneNumberUpdateDto();
        _mockPhoneNumberManager.Setup(m => m.AssignUnassignPhoneNumberAsync(It.IsAny<Guid>(), It.IsAny<PhoneNumberUpdateDto>())).ReturnsAsync(_phoneNumber);

        // Act
        var response = await _httpClient.PatchAsJsonAsync($"/api/phoneNumbers/{_phoneNumber.Id}", phoneNumberUpdateDto);
        response.EnsureSuccessStatusCode();

        var actualPhoneNumber = await response.Content.ReadFromJsonAsync<PhoneNumber>();

        // Assert
        _mockPhoneNumberManager.Verify(m => m.AssignUnassignPhoneNumberAsync(
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

    //[Test]
    //public async Task UpdatePhoneNumberAsync_Should_Return_Error_From_Manager()
    //{
    //    // Arrange
    //    var phoneNumberUpdateDto = new PhoneNumberUpdateDto();
    //    _mockPhoneNumberManager.Setup(m => m.AssignPhoneNumberAsync(It.IsAny<Guid>(), It.IsAny<PhoneNumberUpdateDto>())).ReturnsAsync(_callError);

    //    // Act & Assert Exception
    //    var response = await _httpClient.PatchAsJsonAsync($"/api/phoneNumbers/{_phoneNumber.Id}", phoneNumberUpdateDto);
    //    var ex = Assert.Throws<HttpRequestException>(() => response.EnsureSuccessStatusCode());
    //    var error = await response.Content.ReadFromJsonAsync<ErrorDto>();

    //    // Assert
    //    Assert.Multiple(() =>
    //    {
    //        Assert.That(error.Code, Is.EqualTo(ErrorCodes.VOICEFLEX_0000.ToString()));
    //        Assert.That(error.Message, Is.EqualTo("A resource with this id could not be found."));
    //    });
    //}

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

    [Test]
    public async Task DeletePhoneNumberAsync_Should_Return_Error_From_Manager()
    {
        // Arrange
        var phoneNumberId = Guid.NewGuid();
        _mockPhoneNumberManager.Setup(m => m.DeletePhoneNumberAsync(It.IsAny<Guid>())).ReturnsAsync(_callError);

        // Act & Assert Exception
        var response = await _httpClient.DeleteAsync($"/api/phonenumbers/{phoneNumberId}");
        var ex = Assert.Throws<HttpRequestException>(() => response.EnsureSuccessStatusCode());
        var error = await response.Content.ReadFromJsonAsync<ErrorDto>();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(error.Code, Is.EqualTo(ErrorCodes.VOICEFLEX_0000.ToString()));
            Assert.That(error.Message, Is.EqualTo("A resource with this id could not be found."));
        });
    }

    [TearDown]
    public void TearDown()
    {
        _factory.Dispose();
        _httpClient.Dispose();
    }
}
