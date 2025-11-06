using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.API;
using SchoolManagement.API.Middleware;
using SchoolManagement.Infrastructure.Data;
using SchoolManagement.Persistence;
using Serilog;
using System.Threading.RateLimiting;

public partial class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Host.UseSerilog((context, configuration) =>
        {
            // Read all Serilog configuration from appsettings.json
            configuration.ReadFrom.Configuration(context.Configuration);
        });

        // ------------------- Configuration -------------------
        builder.Configuration
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

        var startup = new Startup(builder.Configuration);
        startup.ConfigureServices(builder.Services);
        //startup.ConfigureMenuServices(builder.Services);


        var app = builder.Build();
        startup.Configure(app, app.Environment);

        // Database Migration and Seeding
        await InitializeDatabaseAsync(app);

        

        //// Health Check Endpoint
        //app.UseHealthChecks("/health");

        app.Run();
    }

    private static async Task InitializeDatabaseAsync(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<SchoolManagementDbContext>();

        try
        {
            // Apply migrations
            await context.Database.MigrateAsync();

            // Seed initial data
            await MenuDataSeeder.SeedAsync(context);

            app.Logger.LogInformation("Database initialized successfully");
        }
        catch (Exception ex)
        {
            app.Logger.LogError(ex, "An error occurred while initializing the database");
            throw;
        }
    }
}