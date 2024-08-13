using VoiceFlex.Models;

namespace VoiceFlex.BLL;

public interface IPhoneNumberManager
{
    Task<List<PhoneNumber>> ListPhoneNumbersAsync();
}

public class PhoneNumberManager : IPhoneNumberManager
{
    //private readonly IVoiceFlexService _voiceFlexService;

    //public PhoneNumberManager(IVoiceFlexService voiceFlexService) => _voiceFlexService = voiceFlexService;
    public PhoneNumberManager() { }

    public async Task<List<PhoneNumber>> ListPhoneNumbersAsync()
    {
        return await Task.FromResult(new List<PhoneNumber>
        {
            new() { Id = new Guid(), Number = "123-456-7890" },
            new() { Id = new Guid(), Number = "098-765-4321" }
        });
    }
}
