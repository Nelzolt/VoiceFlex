using VoiceFlex.Data;
using VoiceFlex.DTO;
using VoiceFlex.Models;

namespace VoiceFlex.DAL;

public interface IPhoneNumberAccessor
{
    Task<PhoneNumberDto> CreateAsync(PhoneNumberDto phoneNumber);
}

public class PhoneNumberAccessor : IPhoneNumberAccessor
{
    private readonly ApplicationDbContext _dbContext;

    public PhoneNumberAccessor(ApplicationDbContext dbContext)
        => _dbContext = dbContext;

    public async Task<PhoneNumberDto> CreateAsync(PhoneNumberDto phoneNumber)
    {
        var dbPhoneNumber = new PhoneNumber(phoneNumber);
        await _dbContext.VOICEFLEX_PhoneNumbers.AddAsync(dbPhoneNumber);
        await _dbContext.SaveChangesAsync();
        phoneNumber.Id = dbPhoneNumber.Id;
        return phoneNumber;
    }
}
