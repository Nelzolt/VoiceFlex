//using Microsoft.AspNetCore.Mvc.Testing;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.VisualStudio.TestPlatform.TestHost;
//using Moq;
//using System.Text.Json;
//using VoiceFlex.BLL;
//using VoiceFlex.Models;

//namespace VoiceFlex.Tests
//{
//    [TestFixture]
//    public class ApiEndpointsTests
//    {
//        private WebApplicationFactory<Program> _factory;

//        [SetUp]
//        public void SetUp()
//        {
//            _factory = new WebApplicationFactory<Program>();
//        }

//        [TearDown]
//        public void TearDown()
//        {
//            _factory.Dispose();
//        }

//        [Test]
//        public async Task ListPhoneNumbersAsync_ReturnsExpectedPhoneNumbers()
//        {
//            // Arrange
//            var expectedPhoneNumbers = new List<PhoneNumber>
//            {
//                new() { Id = new Guid(), Number = "123-456-7890" },
//                new() { Id = new Guid(), Number = "098-765-4321" }
//            };

//            var phoneNumberManagerMock = new Mock<IPhoneNumberManager>();
//            phoneNumberManagerMock.Setup(m => m.ListPhoneNumbersAsync()).ReturnsAsync(expectedPhoneNumbers);

//            HttpClient client = null;
//            try
//            {
//                client = _factory.WithWebHostBuilder(builder =>
//                {
//                    builder.ConfigureServices(services =>
//                    {
//                        services.AddSingleton(phoneNumberManagerMock.Object);
//                    });
//                }).CreateClient();
//            }
//            catch (Exception ex)
//            {
//            }

//            // Act
//            var response = await client.GetAsync("/api/phonenumbers");
//            response.EnsureSuccessStatusCode();

//            var json = await response.Content.ReadAsStringAsync();
//            var actualPhoneNumbers = JsonSerializer.Deserialize<List<PhoneNumber>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

//            // Assert
//            Assert.That(actualPhoneNumbers, Has.Count.EqualTo(expectedPhoneNumbers.Count));
//            Assert.Multiple(() =>
//            {
//                Assert.That(actualPhoneNumbers[0].Number, Is.EqualTo(expectedPhoneNumbers[0].Number));
//                Assert.That(actualPhoneNumbers[1].Number, Is.EqualTo(expectedPhoneNumbers[1].Number));
//            });
//        }
//    }
//}
