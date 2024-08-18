using VoiceFlex.BLL;
using VoiceFlex.DTO;

namespace VoiceFlex.Tests.BLL;

public class ValidatorTests
{
    protected void AssertValidationResult(CallError error, bool isValid, ErrorCodes expectedErrorCode)
    {
        if (isValid)
        {
            Assert.That(error, Is.Null);
        }
        else
        {
            Assert.That(error, Is.Not.Null);
            Assert.That(error.Code, Is.EqualTo(expectedErrorCode));
        }
    }
}
