using Microsoft.EntityFrameworkCore;
using VoiceFlex.BLL;
using VoiceFlex.Data;
using VoiceFlex.DTO;
using VoiceFlex.Models;

namespace VoiceFlex.DAL;

public interface IAccountAccessor
{
    Task<ICallResult> CreateAsync(AccountDto account);
    Task<AccountDto> GetAsync(Guid id);
    Task<ICallResult> SetActiveAsync(Guid id);
    Task<ICallResult> SetSuspendedAsync(Guid id);
}

public class AccountAccessor : IAccountAccessor
{
    private readonly ApplicationDbContext _dbContext;

    public AccountAccessor(ApplicationDbContext dbContext)
        => _dbContext = dbContext;

    public async Task<ICallResult> CreateAsync(AccountDto account)
    {
        var dbAccount = new Account(account);
        await _dbContext.VOICEFLEX_Accounts.AddAsync(dbAccount);
        await _dbContext.SaveChangesAsync();
        return dbAccount;
    }

    public async Task<AccountDto> GetAsync(Guid id)
        => await _dbContext.VOICEFLEX_Accounts
            .AsNoTracking()
            .Where(a => a.Id.Equals(id))
            .Include(a => a.PhoneNumbers)
            .Select(a => new AccountDto(a.Id, a.Description, a.Status, a.PhoneNumbers))
            .FirstOrDefaultAsync();

    public async Task<ICallResult> SetActiveAsync(Guid id)
    {
        var dbAccount = await _dbContext.VOICEFLEX_Accounts.FindAsync(id);
        if (dbAccount is null)
        {
            return new CallError(ErrorCodes.VOICEFLEX_0001);
        }
        dbAccount.Status = AccountStatus.Active;
        await _dbContext.SaveChangesAsync();
        return dbAccount;
    }

    public async Task<ICallResult> SetSuspendedAsync(Guid id)
    {
        var dbAccount = await _dbContext.VOICEFLEX_Accounts
            .Where(a => a.Id.Equals(id))
            .Include(a => a.PhoneNumbers)
            .FirstOrDefaultAsync();
        if (dbAccount is null)
        {
            return new CallError(ErrorCodes.VOICEFLEX_0001);
        }
        dbAccount.Status = AccountStatus.Suspended;
        foreach (var phoneNumber in dbAccount.PhoneNumbers)
        {
            phoneNumber.AccountId = null;
        }
        await _dbContext.SaveChangesAsync();
        return dbAccount;
    }
}
