using ContentGenerator.Data;
using ContentGenerator.Models.Authentication;
using ContentGenerator.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    var secret = builder.Configuration["JwtConfig:Secret"];
    var issuer = builder.Configuration["JwtConfig:ValidIssuer"];
    var audience = builder.Configuration["JwtConfig:ValidAudiences"];
    if (secret is null || issuer is null || audience is null)
    {
        throw new ApplicationException("Jwt is not set in the configuration");
    }
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = audience,
        ValidIssuer = issuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
    };
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSingleton<IContentDbService, MongoDbService>();
builder.Services.AddSingleton<IUserDbService, MongoDbService>();
builder.Services.AddScoped<IContentService, ContentService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddSingleton<ITextGeneratorService, GemeniTextGeneratorService>();
//builder.Services.AddSingleton<IAudioGeneratorService, MurfAudioGeneratorService>();
builder.Services.AddSingleton<IAudioGeneratorService, EleveLabsAudioGeneratorService>();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddHttpClient("Gemeni", httpClient =>
{
    httpClient.BaseAddress = new Uri("https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key=AIzaSyBLnku1vKuOvMDrNJZtzFgllyjuk2LWEL4");

    // using Microsoft.Net.Http.Headers;
    // The GitHub API requires two headers.
    httpClient.DefaultRequestHeaders.Add(
        HeaderNames.Accept, "application/vnd.github.v3+json");
    httpClient.DefaultRequestHeaders.Add(
        HeaderNames.UserAgent, "HttpRequestsSample");
});
builder.Services.AddHttpClient("Murf", httpClient =>
{
    httpClient.BaseAddress = new Uri("https://api.murf.ai/v1/speech/generate");
});
builder.Services.AddHttpClient("ElevenLabs", httpClient =>
{
    httpClient.BaseAddress = new Uri("https://api.elevenlabs.io/v1/text-to-speech");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
