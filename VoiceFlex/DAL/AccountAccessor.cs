using Microsoft.EntityFrameworkCore;
using VoiceFlex.Data;
using VoiceFlex.DTO;

namespace VoiceFlex.DAL;

public interface IAccountAccessor
{
    Task<AccountDto> GetAsync(Guid id);
}

public class AccountAccessor : IAccountAccessor
{
    private readonly ApplicationDbContext _dbContext;

    public AccountAccessor(ApplicationDbContext dbContext)
        => _dbContext = dbContext;

    public async Task<AccountDto> GetAsync(Guid id)
    {
        return await Task.FromResult(new AccountDto());
    }
        //=> await _dbContext.VOICEFLEX_PhoneNumbers
        //    .AsNoTracking()
        //    .OrderBy(p => p.Number)
        //    .Select(p => new PhoneNumberDto(p.Id, p.Number, p.AccountId))
        //    .ToListAsync();
}
