using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Site.Data;
using Site.Data.Model;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddSimpleConsole(opt =>
{
    opt.SingleLine = true;
    opt.UseUtcTimestamp = true;
});
//builder.Logging.AddConsole();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddScoped<WorkitemService>();
builder.Services.AddDbContextFactory<WorkitemContext>(opt =>
        opt.UseSqlite($"Data Source=database.sqlite")
        .EnableSensitiveDataLogging());

//builder.Services.AddDbContextFactory<WorkitemContext>(opt =>
//    opt.UseInMemoryDatabase(Guid.NewGuid().ToString())
//    .EnableSensitiveDataLogging());
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();