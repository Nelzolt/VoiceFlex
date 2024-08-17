using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using System.Reflection;
using VoiceFlex.DTO;

namespace VoiceFlex.Tests.ApiEndpoints;

[TestFixture]
public class ApiEndpointsTests
{
    private WebApplicationFactory<Program> _factory;
    private HttpClient _httpClient;

    [SetUp]
    public void SetUp()
    {
        _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
        });
        _httpClient = _factory.CreateClient();
        var accountId = Guid.NewGuid();
    }

    [Test]
    public async Task ServiceAlive_Should_Return_ServiceAlive_Dto()
    {
        // Act
        var response = await _httpClient.GetAsync("/api");
        response.EnsureSuccessStatusCode();

        var serviceAlive = await response.Content.ReadFromJsonAsync<ServiceAlive>();

        // Assert
        Assert.That(serviceAlive.Version, Is.EqualTo(Assembly.GetEntryAssembly().GetName().Version.ToString()));
    }

    [TearDown]
    public void TearDown()
    {
        _factory.Dispose();
        _httpClient.Dispose();
    }
}
