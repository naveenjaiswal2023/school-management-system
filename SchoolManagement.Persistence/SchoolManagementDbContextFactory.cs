using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace SchoolManagement.Persistence
{
    public class SchoolManagementDbContextFactory : IDesignTimeDbContextFactory<SchoolManagementDbContext>
    {
        public SchoolManagementDbContext CreateDbContext(string[] args)
        {
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "../SchoolManagement.API");

            // Build configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile(
                    $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development"}.json",
                    optional: true,
                    reloadOnChange: true)
                .Build();

            // Get connection string
            var connectionString = configuration.GetConnectionString("SchoolManagementDbConnectionString");
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new InvalidOperationException("Connection string 'SchoolManagementDbConnectionString' not found.");

            // Configure DbContext
            var optionsBuilder = new DbContextOptionsBuilder<SchoolManagementDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new SchoolManagementDbContext(optionsBuilder.Options);
        }
    }
}
