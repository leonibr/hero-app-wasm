using HeroApp.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace HeroApp.Infra
{
    public class DesignTimeContext : IDesignTimeDbContextFactory<HeroContext>
    {
        public HeroContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "..", "HeroApp.Api"))
                .AddJsonFile($"appsettings.{Environments.Development}.json")
                .Build();

            var builder = new DbContextOptionsBuilder<HeroContext>();

            var connectionString = configuration.GetConnectionString("Sqlite");

            builder.UseSqlite(connectionString, x =>
                x.MigrationsHistoryTable(Constants.HistoryTableName));

            return new HeroContext(options: builder.Options,
                currentUserService: default,
                dateTime: default);
        }
    }
}
