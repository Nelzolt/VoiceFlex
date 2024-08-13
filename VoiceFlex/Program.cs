using VoiceFlex;
using VoiceFlex.BLL;

var builder = WebApplication.CreateBuilder(args);

#region BLL Managers

builder.Services.AddScoped<IPhoneNumberManager, PhoneNumberManager>();

#endregion

var app = builder.Build();

app.MapApiEndpoints()
    .UseHttpsRedirection();

app.Run();
