using Microsoft.IdentityModel.Tokens;
using System.Text;
using BAL.IServices;
using BAL.Services;
using COMMON;
using DAL.DapperAccess;
using Microsoft.AspNetCore.Authentication.JwtBearer;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
var environment = builder.Environment;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
            .GetBytes(builder.Configuration.GetSection("Configuration:JWT:PrivateKey").Value!)),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<Configuration>(builder.Configuration.GetSection("Configuration"));

builder.Services.AddTransient<DapperAccess>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<BAL.BAL>();

WebApplication app = builder.Build();

if (environment.IsProduction())
{
    string port = Environment.GetEnvironmentVariable("PORT") ?? throw new ArgumentNullException($"Application port number is missing");
    app.Urls.Add($"http://*:{port}");
}

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
