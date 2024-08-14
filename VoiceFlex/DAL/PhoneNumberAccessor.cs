using VoiceFlex.Data;
using VoiceFlex.DTO;
using VoiceFlex.Models;

namespace VoiceFlex.DAL;

public interface IPhoneNumberAccessor
{
    Task<PhoneNumberDto> CreateAsync(PhoneNumberDto phoneNumber);
    Task<PhoneNumber> UpdateAsync(Guid id, PhoneNumberUpdateDto phoneNumberUpdate);
    Task DeleteAsync(Guid id);
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

    public async Task<PhoneNumber> UpdateAsync(Guid id, PhoneNumberUpdateDto phoneNumberUpdate)
    {
        var dbPhoneNumber = await _dbContext.VOICEFLEX_PhoneNumbers.FindAsync(id);
        dbPhoneNumber.AccountId = phoneNumberUpdate.AccountId;
        await _dbContext.SaveChangesAsync();
        return dbPhoneNumber;
    }

    public async Task DeleteAsync(Guid id)
    {
        var phoneNumber = await _dbContext.VOICEFLEX_PhoneNumbers.FindAsync(id);
        _dbContext.VOICEFLEX_PhoneNumbers.Remove(phoneNumber);
        await _dbContext.SaveChangesAsync();
    }
}
