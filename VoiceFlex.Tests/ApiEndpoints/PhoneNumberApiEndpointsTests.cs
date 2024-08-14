using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net.Http.Json;
using VoiceFlex.BLL;
using VoiceFlex.DTO;
using VoiceFlex.Tests.TestHelpers;

namespace VoiceFlex.Tests.ApiEndpoints;

[TestFixture]
public class PhoneNumberApiEndpointsTests
{
    private WebApplicationFactory<Program> _factory;
    private HttpClient _httpClient;
    private Mock<IPhoneNumberManager> _mockPhoneNumberManager;
    private PhoneNumberDto _expectedPhoneNumber;

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
    }

    [Test]
    public async Task CreateAccountAsync_Should_Call_AccountManager_CreateAccountAsync_And_Return_NewAccount()
    {
        // Arrange
        _mockPhoneNumberManager.Setup(m => m.CreatePhoneNumberAsync(It.IsAny<PhoneNumberDto>())).ReturnsAsync(_expectedPhoneNumber);

        // Act
        var response = await _httpClient.PostAsJsonAsync($"/api/phonenumbers", _expectedPhoneNumber);
        response.EnsureSuccessStatusCode();

        var actualPhoneNumber = await response.Content.ReadFromJsonAsync<PhoneNumberDto>();

        // Assert
        _mockPhoneNumberManager.Verify(m => m.CreatePhoneNumberAsync(It.Is<PhoneNumberDto>(
            p => p.Number.Equals(_expectedPhoneNumber.Number))),
            Times.Once);
        Assert.That(actualPhoneNumber.Number, Is.EqualTo(_expectedPhoneNumber.Number));
    }

    [TearDown]
    public void TearDown()
    {
        _factory.Dispose();
        _httpClient.Dispose();
    }
}
