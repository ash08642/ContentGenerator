using ContentGenerator.Data;
using ContentGenerator.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<IContentService, ContentService>();
builder.Services.AddSingleton<IHttpClientService, HttpClientService>();
builder.Services.AddSingleton<ITextGeneratorService, TextGeneratorService>();

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

builder.Services.AddDbContext<ContentDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
