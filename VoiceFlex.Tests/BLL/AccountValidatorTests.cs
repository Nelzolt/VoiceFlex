using VoiceFlex.BLL;
using VoiceFlex.DTO;

namespace VoiceFlex.Tests.BLL;

public class AccountValidatorTests : ValidatorTests
{
    private AccountValidator _accountValidator;

    [SetUp]
    public void SetUp()
    {
        _accountValidator = new AccountValidator();
    }

    [TestCase(true, true)]
    [TestCase(false, false)]
    public void FoundInDatabase_Should_Return_Correct_Result(bool entityFound, bool isValid)
    {
        // Arrange
        var _account = entityFound ? new AccountDto() : null;

        // Act
        var error = _accountValidator.FoundInDatabase(_account).ErrorFound as CallError;

        // Assert
        AssertValidationResult(error, isValid, ErrorCodes.VOICEFLEX_0000);
    }

    [TestCase(null, false)]
    [TestCase("", false)]
    [TestCase("a", true)]
    [TestCase("t1huIlqTkA3Y4VF82pZHpCcOoWlmJFQHJ9RPuDW2o3qWLL81RDA41CwswQ4S6PjEhBo101RJIF5pf3YzDVWVY43hjqwpBAbWgQhAeY48UORVAOtlONbcmM6D2Bp86xKhwOEU5GLEaurSU6xYpkyn2iZRE8sxEiYzMKnS1rzFxUpFBL3VLhf0xCPXh6SusYtJwg3SSNoC47JWirGupvHuZOCSq3IT417ORTdmnXwraqzsAuOoUR3ZzgEv81IRJRiylnDgPu6uxIqkYYGZsVVr7ISLdO2wGpyEG5cgqHxObjbpm7PckvCykMN0wY61Zi0Mmg2cFccvg9ngJu24wkKOCdDoVuLhGY79zAGEAeaUq4MM4cp1SB7MnvrCJ8dhCT7I0tZdA3Wv0pzSHYH1J1sS8eSWyv83xNAHe1EK5YYcvbeZSLSkvn64xdXybrWae0YOdHJ7s1XF04ehDYliJ9uIbmkzUijyq97wpnXAsixrAZrBkuxUnHD1kjSAfqIgT1cBKdfzsAnKfAz2KcoOp9YqL7WCnE41kRKmbhJdk35A53AuGXD8MVZKcnIcSQRmvikwUtVZcef1h6mFTHewTyRcIriZRq7OV6QU4hQDSCcIR29s16rNl9HnNgcFqISIia2XK5jLnuIV2esGlRdCEQPGB48vivdR5expQUueZwZcGVKIXf4JpbL3RuiScItW2eUVkhfbgwgG5i4PxvB3aJ9HQqJhocZWyF9IyXqsyjxWhjz6wDDgEkpCkvLioDDiSThTg35JTw644EeUzkFmAOq47VaySsEelqs3qzrgPOaFNfartjua8KbtXwiVRGfwUSHycAhOAVhjbJrzTa1vKp2ViDmk9GbLfXaggibnsAfYgDDhfe7RHD44XIDTRsrJLQtJT9GfYgwCUzfTJjZ7gSMEw3iMuFKN2wDNaLX0AkfKJx1Fk57YlSyUmETBvUEqOkjvZvU4zevMypyUGAvkWAbolmIIUZiHhCHRxSsrNrz7nf6KCzCNSEmvkrk0yvTr51G",
        true)]
    [TestCase("et1huIlqTkA3Y4VF82pZHpCcOoWlmJFQHJ9RPuDW2o3qWLL81RDA41CwswQ4S6PjEhBo101RJIF5pf3YzDVWVY43hjqwpBAbWgQhAeY48UORVAOtlONbcmM6D2Bp86xKhwOEU5GLEaurSU6xYpkyn2iZRE8sxEiYzMKnS1rzFxUpFBL3VLhf0xCPXh6SusYtJwg3SSNoC47JWirGupvHuZOCSq3IT417ORTdmnXwraqzsAuOoUR3ZzgEv81IRJRiylnDgPu6uxIqkYYGZsVVr7ISLdO2wGpyEG5cgqHxObjbpm7PckvCykMN0wY61Zi0Mmg2cFccvg9ngJu24wkKOCdDoVuLhGY79zAGEAeaUq4MM4cp1SB7MnvrCJ8dhCT7I0tZdA3Wv0pzSHYH1J1sS8eSWyv83xNAHe1EK5YYcvbeZSLSkvn64xdXybrWae0YOdHJ7s1XF04ehDYliJ9uIbmkzUijyq97wpnXAsixrAZrBkuxUnHD1kjSAfqIgT1cBKdfzsAnKfAz2KcoOp9YqL7WCnE41kRKmbhJdk35A53AuGXD8MVZKcnIcSQRmvikwUtVZcef1h6mFTHewTyRcIriZRq7OV6QU4hQDSCcIR29s16rNl9HnNgcFqISIia2XK5jLnuIV2esGlRdCEQPGB48vivdR5expQUueZwZcGVKIXf4JpbL3RuiScItW2eUVkhfbgwgG5i4PxvB3aJ9HQqJhocZWyF9IyXqsyjxWhjz6wDDgEkpCkvLioDDiSThTg35JTw644EeUzkFmAOq47VaySsEelqs3qzrgPOaFNfartjua8KbtXwiVRGfwUSHycAhOAVhjbJrzTa1vKp2ViDmk9GbLfXaggibnsAfYgDDhfe7RHD44XIDTRsrJLQtJT9GfYgwCUzfTJjZ7gSMEw3iMuFKN2wDNaLX0AkfKJx1Fk57YlSyUmETBvUEqOkjvZvU4zevMypyUGAvkWAbolmIIUZiHhCHRxSsrNrz7nf6KCzCNSEmvkrk0yvTr51G",
        false)]
    public void DescriptionMustBeValid_Should_Return_Correct_Result(string description, bool isValid)
    {
        // Act
        var error = _accountValidator.DescriptionMustBeValid(description).ErrorFound as CallError;

        // Assert
        AssertValidationResult(error, isValid, ErrorCodes.VOICEFLEX_0005);
    }
}
