using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Json.Serialization;
using VoiceFlex.ApiEndpoints;
using VoiceFlex.BLL;
using VoiceFlex.DAL;
using VoiceFlex.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "VoiceFlex API",
        Version = "v1"
    });
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.Configure<JsonOptions>(options =>
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault | JsonIgnoreCondition.WhenWritingNull);

#region Database

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

#endregion
#region BLL Managers

builder.Services.AddScoped<IAccountManager, AccountManager>();
builder.Services.AddScoped<IAccountValidator, AccountValidator>();
builder.Services.AddScoped<IPhoneNumberManager, PhoneNumberManager>();
builder.Services.AddScoped<IPhoneNumberValidator, PhoneNumberValidator>();

builder.Services.AddScoped<IErrorManager, ErrorManager>();

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
