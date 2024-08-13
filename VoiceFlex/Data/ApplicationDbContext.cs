using Microsoft.EntityFrameworkCore;
using VoiceFlex.Models;

namespace VoiceFlex.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Account> VOICEFLEX_Accounts { get; set; }

        public DbSet<PhoneNumber> VOICEFLEX_PhoneNumbers { get; set; }
    }
}
