using Microsoft.EntityFrameworkCore;
using VoiceFlex.Data;
using VoiceFlex.DTO;

namespace VoiceFlex.DAL
{
    public interface IPhoneNumberAccessor
    {
        Task<List<PhoneNumberDto>> ListAsync();
    }

    public class PhoneNumberAccessor : IPhoneNumberAccessor
    {
        private readonly ApplicationDbContext _dbContext;

        public PhoneNumberAccessor(ApplicationDbContext dbContext) => _dbContext = dbContext;

        public async Task<List<PhoneNumberDto>> ListAsync()
            => await _dbContext.VOICEFLEX_PhoneNumbers
                .AsNoTracking()
                .OrderBy(c => c.Number)
                .Select(c => new PhoneNumberDto(c.Id, c.Number))
                .ToListAsync();
    }
}
