using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;

namespace Common.Data
{
    public class DbContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        IConfigurationRoot _config;
        readonly string _environmentName;
        readonly string _basePath;

        public DbContextFactory()
        {
            _basePath = AppContext.BaseDirectory;
            _environmentName = Environment.GetEnvironmentVariable("Hosting:Environment");

            var builder = new ConfigurationBuilder()
              .SetBasePath(_basePath)
              .AddJsonFile("appsettings.json")
              .AddJsonFile($"appsettings.{_environmentName}.json", true)
              .AddEnvironmentVariables();

            _config = builder.Build();
        }

        public DataContext CreateDbContext(string[] args)
            => Create(_config.GetConnectionString("DefaultConnection"));

        public DataContext CreateDbContext(string connectionString)
                   => Create(connectionString);
        DataContext Create(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException($"{nameof(connectionString)} is null or empty.", nameof(connectionString));
            }

            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();

            optionsBuilder.UseSqlServer(connectionString);

            return new DataContext(optionsBuilder.Options);
        }
    }
}
