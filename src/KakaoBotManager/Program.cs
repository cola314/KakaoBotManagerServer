using KakaoBotManager.Config;
using KakaoBotManager.Repository;
using KakaoBotManager.Services;
using KakaoBotManager.Storage;
using KakaoBotManager.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddControllers();

builder.Services
    .AddScoped<UserInteraction>()
    .AddScoped<ITokenStorage, TokenStorage>();

builder.Services
    .AddTransient<AuthService>()
    .AddTransient<AddressService>();

builder.Services
    .AddSingleton<AuthRepository>()
    .AddSingleton<TokenRepository>()
    .AddSingleton<IAddressRepository, AddressRepository>();

builder.Services
    .AddSingleton<ApiKeyService>()
    .AddSingleton<KakaoBotService>();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSingleton<IEnvironmentConfig, DevelopmentEnvironmentConfig>();
}
else
{
    builder.Services.AddSingleton<IEnvironmentConfig, EnvironmentConfig>();
}

var app = builder.Build();

// Eager loading to check enviroment variable is valid
app.Services.GetService<IEnvironmentConfig>();
app.Services.GetService<KakaoBotService>().Run();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
