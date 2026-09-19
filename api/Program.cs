using api.Data;
using Microsoft.EntityFrameworkCore;
using api.Interfaces;
using api.Repository;
using api.Models;
using api.Services;
using api.ExternalProviders;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IFoodEntryRepository, FoodEntryRepository>();
builder.Services.AddScoped<INutritionCalculator, NutritionCalculator>();
builder.Services.AddScoped<IFoodService, FoodService>();


builder.Services.AddHttpClient<
    IExternalFoodProvider,
    OpenFoodFactsProvider>((services, client) =>
        {
            var configuration =
                services.GetRequiredService<IConfiguration>();

            var baseUrl = configuration[
                "ExternalFoodApis:OpenFoodFacts:BaseUrl"]
                ?? throw new InvalidOperationException(
                    "Open Food Facts BaseUrl is missing");

            var userAgent = configuration[
                "ExternalFoodApis:OpenFoodFacts:UserAgent"]
                ?? throw new InvalidOperationException(
                    "Open Food Facts UserAgent is missing");

            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);
            client.Timeout = TimeSpan.FromSeconds(10);
        });

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    );
});

builder.Services
    .AddIdentityApiEndpoints<AppUser>()
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{                                              
    app.MapOpenApi();
}



app.UseCors("Frontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapIdentityApi<AppUser>();
app.MapControllers();

app.Run();
