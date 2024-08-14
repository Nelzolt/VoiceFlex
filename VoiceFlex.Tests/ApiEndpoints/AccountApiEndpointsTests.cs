﻿using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net.Http.Json;
using VoiceFlex.BLL;
using VoiceFlex.DTO;
using VoiceFlex.Tests.TestHelpers;

namespace VoiceFlex.Tests.ApiEndpoints;

[TestFixture]
public class AccountApiEndpointsTests
{
    private WebApplicationFactory<Program> _factory;
    private HttpClient _httpClient;
    private Mock<IAccountManager> _mockAccountManager;
    private AccountDto _expectedAccount;

    [SetUp]
    public void SetUp()
    {
        _mockAccountManager = new Mock<IAccountManager>();
        _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.AddSingleton(_mockAccountManager.Object);
        });
        _httpClient = _factory.CreateClient();
        var accountId = Guid.NewGuid();
        _expectedAccount = new AccountDto()
        {
            Id = accountId,
            Description = "test",
            PhoneNumbers = new List<PhoneNumberDto>
            {
                new(Guid.NewGuid(), "0987654321", accountId),
                new(Guid.NewGuid(), "1234567890", accountId)
            }
        };
    }

    [Test]
    public async Task CreateAccountAsync_Should_Call_AccountManager_CreateAccountAsync_And_Return_NewAccount()
    {
        // Arrange
        _mockAccountManager.Setup(m => m.CreateAccountAsync(It.IsAny<AccountDto>())).ReturnsAsync(_expectedAccount);

        // Act
        var response = await _httpClient.PostAsJsonAsync($"/api/accounts", _expectedAccount);
        response.EnsureSuccessStatusCode();

        var actualAccount = await response.Content.ReadFromJsonAsync<AccountDto>();

        // Assert
        _mockAccountManager.Verify(m => m.CreateAccountAsync(It.Is<AccountDto>(
            p => p.Description.Equals(_expectedAccount.Description)
            && p.Status.Equals(_expectedAccount.Status))),
            Times.Once);
        Assert.Multiple(() =>
        {
            Assert.That(actualAccount.Description, Is.EqualTo(_expectedAccount.Description));
            Assert.That(actualAccount.Status, Is.EqualTo(_expectedAccount.Status));
        });
    }

    [Test]
    public async Task GetAccountWithPhoneNumbersAsync_ReturnsExpectedAccountWithPhoneNumbers()
    {
        // Arrange
        _mockAccountManager.Setup(m => m.GetAccountWithPhoneNumbersAsync(It.IsAny<Guid>())).ReturnsAsync(_expectedAccount);

        // Act
        var response = await _httpClient.GetAsync($"/api/accounts/{_expectedAccount.Id}/phonenumbers");
        response.EnsureSuccessStatusCode();

        var actualAccount = await response.Content.ReadFromJsonAsync<AccountDto>();
        var actualPhoneNumbers = actualAccount.PhoneNumbers;

        // Assert
        Assert.That(actualPhoneNumbers, Has.Count.EqualTo(_expectedAccount.PhoneNumbers.Count));
        Assert.Multiple(() =>
        {
            Assert.That(actualAccount.Id, Is.EqualTo(_expectedAccount.Id));
            Assert.That(actualPhoneNumbers[0].Id, Is.EqualTo(_expectedAccount.PhoneNumbers[0].Id));
            Assert.That(actualPhoneNumbers[0].Number, Is.EqualTo(_expectedAccount.PhoneNumbers[0].Number));
            Assert.That(actualPhoneNumbers[0].AccountId, Is.EqualTo(_expectedAccount.PhoneNumbers[0].AccountId));
            Assert.That(actualPhoneNumbers[1].Id, Is.EqualTo(_expectedAccount.PhoneNumbers[1].Id));
            Assert.That(actualPhoneNumbers[1].Number, Is.EqualTo(_expectedAccount.PhoneNumbers[1].Number));
            Assert.That(actualPhoneNumbers[1].AccountId, Is.EqualTo(_expectedAccount.PhoneNumbers[1].AccountId));
        });
    }

    [TearDown]
    public void TearDown()
    {
        _factory.Dispose();
        _httpClient.Dispose();
    }
}
