using Microsoft.EntityFrameworkCore;
using VoiceFlex.ApiEndpoints;
using VoiceFlex.BLL;
using VoiceFlex.DAL;
using VoiceFlex.Data;

var builder = WebApplication.CreateBuilder(args);

#region Database

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

#endregion
#region BLL Managers

builder.Services.AddScoped<IPhoneNumberManager, PhoneNumberManager>();

#endregion
#region Data Accessors

builder.Services.AddScoped<IPhoneNumberAccessor, PhoneNumberAccessor>();

#endregion

builder.Build()
    .MapPhoneNumberApiEndpoints()
    .MapApiEndpoints()
    .Run();

// This line is needed for NUnit WebApplication integration tests
public partial class Program { }
