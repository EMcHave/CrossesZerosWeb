using CrossesZerosWeb.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CrossesZerosWeb;
using System;

using Microsoft.Extensions.Hosting.WindowsServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Host.UseWindowsService();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

// See https://aka.ms/new-console-template for more information


var DBbuilder = Host.CreateApplicationBuilder(args);

var connectionString = DBbuilder.Configuration.GetConnectionString("DefaultConnection");

DBbuilder.Services.AddDbContext<AppDbContext>(options =>
	options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention());
DBbuilder.Build();


/*
var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
var options = optionsBuilder
	.UseNpgsql(connectionString)
.UseSnakeCaseNamingConvention()
.Options;

using AppDbContext dbContext = new AppDbContext(options);

/*
dbContext.GameResults.Add(new GameResult()
{
	Winner = "Zeros",
	Moves = 10
});

dbContext.SaveChanges();

var results = dbContext.GameResults.ToArray();

foreach (var result in results)
{
	Console.WriteLine(result.Id);
}
*/