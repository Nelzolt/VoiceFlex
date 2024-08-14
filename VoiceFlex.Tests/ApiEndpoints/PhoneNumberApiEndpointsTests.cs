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
    private List<PhoneNumberDto> _expectedPhoneNumbers;

    [SetUp]
    public void SetUp()
    {
        _mockPhoneNumberManager = new Mock<IPhoneNumberManager>();
        _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.AddSingleton(_mockPhoneNumberManager.Object);
        });
        _httpClient = _factory.CreateClient();
        _expectedPhoneNumbers = new List<PhoneNumberDto>
        {
            new(Guid.NewGuid(), "0987654321", Guid.NewGuid()),
            new(Guid.NewGuid(), "1234567890", null)
        };
    }

    //[Test]
    //public async Task ListPhoneNumbersAsync_ReturnsExpectedPhoneNumbers()
    //{
    //    // Arrange
    //    _mockPhoneNumberManager.Setup(m => m.ListPhoneNumbersAsync()).ReturnsAsync(_expectedPhoneNumbers);

    //    // Act
    //    var response = await _httpClient.GetAsync("/api/phonenumbers");
    //    response.EnsureSuccessStatusCode();

    //    var actualPhoneNumbers = await response.Content.ReadFromJsonAsync<List<PhoneNumberDto>>();

    //    // Assert
    //    Assert.That(actualPhoneNumbers, Has.Count.EqualTo(_expectedPhoneNumbers.Count));
    //    Assert.Multiple(() =>
    //    {
    //        Assert.That(actualPhoneNumbers[0].Id, Is.EqualTo(_expectedPhoneNumbers[0].Id));
    //        Assert.That(actualPhoneNumbers[0].Number, Is.EqualTo(_expectedPhoneNumbers[0].Number));
    //        Assert.That(actualPhoneNumbers[0].AccountId, Is.EqualTo(_expectedPhoneNumbers[0].AccountId));
    //        Assert.That(actualPhoneNumbers[1].Id, Is.EqualTo(_expectedPhoneNumbers[1].Id));
    //        Assert.That(actualPhoneNumbers[1].Number, Is.EqualTo(_expectedPhoneNumbers[1].Number));
    //        Assert.That(actualPhoneNumbers[1].AccountId, Is.EqualTo(_expectedPhoneNumbers[1].AccountId));
    //    });
    //}

    [TearDown]
    public void TearDown()
    {
        _factory.Dispose();
        _httpClient.Dispose();
    }
}
