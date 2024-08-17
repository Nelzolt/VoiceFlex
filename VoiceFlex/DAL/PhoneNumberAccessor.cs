using Microsoft.EntityFrameworkCore;
using VoiceFlex.Data;
using VoiceFlex.DTO;
using VoiceFlex.Models;

namespace VoiceFlex.DAL;

public interface IPhoneNumberAccessor
{
    Task<PhoneNumber> CreateAsync(PhoneNumberDto phoneNumber);
    Task<PhoneNumber> GetAsync(Guid id);
    Task<PhoneNumber> GetByNumberAsync(string number);
    Task<PhoneNumber> AssignUnassignAsync(PhoneNumber phoneNumber, Guid? accountId);
    Task<PhoneNumber> DeleteAsync(Guid id);
}

public class PhoneNumberAccessor : IPhoneNumberAccessor
{
    private readonly ApplicationDbContext _dbContext;

    public PhoneNumberAccessor(ApplicationDbContext dbContext)
        => _dbContext = dbContext;

    public async Task<PhoneNumber> CreateAsync(PhoneNumberDto phoneNumber)
    {
        var dbPhoneNumber = new PhoneNumber(phoneNumber);
        await _dbContext.VOICEFLEX_PhoneNumbers.AddAsync(dbPhoneNumber);
        await _dbContext.SaveChangesAsync();
        return dbPhoneNumber;
    }

    public async Task<PhoneNumber> GetAsync(Guid id)
        => await _dbContext.VOICEFLEX_PhoneNumbers.FindAsync(id);

    public async Task<PhoneNumber> GetByNumberAsync(string number)
        => await _dbContext.VOICEFLEX_PhoneNumbers
            .AsNoTracking()
            .Where(p => p.Number.Equals(number))
            .FirstOrDefaultAsync();

    public async Task<PhoneNumber> AssignUnassignAsync(PhoneNumber phoneNumber, Guid? accountId)
    {
        phoneNumber.AccountId = accountId;
        await _dbContext.SaveChangesAsync();
        return phoneNumber;
    }

    public async Task<PhoneNumber> DeleteAsync(Guid id)
    {
        var phoneNumber = await _dbContext.VOICEFLEX_PhoneNumbers.FindAsync(id);
        if (phoneNumber is not null)
        {
            _dbContext.VOICEFLEX_PhoneNumbers.Remove(phoneNumber);
            await _dbContext.SaveChangesAsync();
        }
        return phoneNumber;
    }
}
