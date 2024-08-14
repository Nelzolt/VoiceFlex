using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using VoiceFlex.ApiEndpoints;
using VoiceFlex.BLL;
using VoiceFlex.DAL;
using VoiceFlex.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "VoiceFlex API",
        Version = "v1"
    });
});
builder.Services.Configure<JsonOptions>(options =>
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault | JsonIgnoreCondition.WhenWritingNull);

#region Database

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

#endregion
#region BLL Managers

builder.Services.AddScoped<IAccountManager, AccountManager>();
builder.Services.AddScoped<IPhoneNumberManager, PhoneNumberManager>();

#endregion
#region Data Accessors

builder.Services.AddScoped<IAccountAccessor, AccountAccessor>();
builder.Services.AddScoped<IPhoneNumberAccessor, PhoneNumberAccessor>();

#endregion

builder.Build()
    .MapAccountApiEndpoints()
    .MapPhoneNumberApiEndpoints()
    .MapApiEndpoints()
    .Run();

// This line is needed for NUnit WebApplication integration tests
public partial class Program { }
