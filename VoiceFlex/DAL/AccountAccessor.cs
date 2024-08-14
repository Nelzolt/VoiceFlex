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
        => await _dbContext.VOICEFLEX_Accounts
            .AsNoTracking()
            .Where(a => a.Id.Equals(id))
            .Include(a => a.PhoneNumbers)
            .Select(a => new AccountDto(a.Id, a.Description, a.Status, a.PhoneNumbers))
            .FirstOrDefaultAsync();
}
