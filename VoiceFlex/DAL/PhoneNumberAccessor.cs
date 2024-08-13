using Microsoft.EntityFrameworkCore;
using VoiceFlex.Data;
using VoiceFlex.DTO;

namespace VoiceFlex.DAL;

public interface IPhoneNumberAccessor
{
    Task<List<PhoneNumberDto>> ListAsync();
}

public class PhoneNumberAccessor : IPhoneNumberAccessor
{
    private readonly ApplicationDbContext _dbContext;

    public PhoneNumberAccessor(ApplicationDbContext dbContext)
        => _dbContext = dbContext;

    public async Task<List<PhoneNumberDto>> ListAsync()
        => await _dbContext.VOICEFLEX_PhoneNumbers
            .AsNoTracking()
            .OrderBy(p => p.Number)
            .Select(p => new PhoneNumberDto(p.Id, p.Number, p.AccountId))
            .ToListAsync();
}
