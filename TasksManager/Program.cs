using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TasksManager.BackgroundServices;
using BAL.IServices;
using BAL.Services;
using COMMON;
using DAL.DapperAccess;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using BAL.Events;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
var environment = builder.Environment;

// Add services to the container.
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
builder.Services.AddScoped<AuthMain>();
builder.Services.AddScoped<AuthEvents>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IUserService, UserService>();

WebApplication app = builder.Build();

if (!environment.IsDevelopment()) 
{
    // Bind to the PORT environment variable required by Heroku
    string port = Environment.GetEnvironmentVariable("PORT") ?? "8080"; // Default to 8080 if PORT not set
    app.Urls.Add($"http://*:{port}");
}

// Configure the HTTP request pipeline.
// app.UseHttpsRedirection();
app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

// Enable Swagger (can be limited to development if desired)
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
