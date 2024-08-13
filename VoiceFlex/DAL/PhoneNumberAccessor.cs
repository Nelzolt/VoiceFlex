using VoiceFlex.Data;
using VoiceFlex.Models;

namespace VoiceFlex.DAL
{
    public interface IPhoneNumberAccessor
    {
        Task<List<PhoneNumber>> ListAsync();
    }

    public class PhoneNumberAccessor : IPhoneNumberAccessor
    {
        private readonly ApplicationDbContext _dbContext;

        public PhoneNumberAccessor(ApplicationDbContext dbContext) => _dbContext = dbContext;
        public async Task<List<PhoneNumber>> ListAsync()
        {
            return await Task.FromResult(new List<PhoneNumber>
            {
                new() { Id = new Guid(), Number = "123-456-7890" },
                new() { Id = new Guid(), Number = "098-765-4321" }
            });
        }
    }
}
