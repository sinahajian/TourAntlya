using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Models.DbContexts
{
    public class TourDbContextFactory : IDesignTimeDbContextFactory<TourDbContext>
    {
        public TourDbContext CreateDbContext(string[] args)
        {
            var basePath = Directory.GetCurrentDirectory();
            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var connectionString = configuration.GetConnectionString("SinaLocalhost")
                ?? "Host=localhost;Port=5432;Database=TourAntalya;Username=postgres;Password=postgres";

            var optionsBuilder = new DbContextOptionsBuilder<TourDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new TourDbContext(optionsBuilder.Options);
        }
    }
}
