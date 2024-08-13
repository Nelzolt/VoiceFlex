using Microsoft.EntityFrameworkCore;
using VoiceFlex;
using VoiceFlex.BLL;
using VoiceFlex.Data;

var builder = WebApplication.CreateBuilder(args);

#region Database

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

#endregion
#region BLL Managers

builder.Services.AddScoped<IPhoneNumberManager, PhoneNumberManager>();

#endregion

var app = builder.Build();

app.MapApiEndpoints()
    .UseHttpsRedirection();

app.Run();
