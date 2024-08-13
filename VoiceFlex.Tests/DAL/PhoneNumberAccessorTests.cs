//using VoiceFlex.BLL;
//using VoiceFlex.Models;

//namespace VoiceFlex.Tests.DAL
//{
//    public class PhoneNumberAccessorTests
//    {
//        private PhoneNumberManager _phoneNumberManager;

//        [SetUp]
//        public void SetUp()
//        {
//            _phoneNumberManager = new PhoneNumberManager();
//        }

//        [Test]
//        public async Task ListPhoneNumbersAsync_ShouldReturnListOfPhoneNumbers()
//        {
//            // Arrange (optional, not needed in this simple test)

//            // Act
//            var result = await _phoneNumberManager.ListPhoneNumbersAsync();

//            // Assert
//            Assert.NotNull(result, "The result should not be null");
//            Assert.IsInstanceOf<List<PhoneNumber>>(result, "The result should be of type List<PhoneNumber>");
//            Assert.AreEqual(2, result.Count, "The list should contain two phone numbers");

//            // You might want to check the content of the phone numbers
//            Assert.AreEqual("123-456-7890", result[0].Number, "The first phone number is incorrect");
//            Assert.AreEqual("098-765-4321", result[1].Number, "The second phone number is incorrect");
//        }
//    }
//}
