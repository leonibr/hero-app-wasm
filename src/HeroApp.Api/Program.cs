using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HeroApp.Domain;
using HeroApp.Infra;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

namespace HeroApp.Api
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {

            Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    // .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                    .Enrich.FromLogContext()
                    .WriteTo.Async(c => c.Console())
                    .CreateLogger();

            try
            {

                Log.Information("Starting web host");
                IHost host = CreateHostBuilder(args).Build();
                ILogger<Program> logger = null;
                using (var scope = host.Services.CreateScope())
                {
                    logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                   
                    var services = scope.ServiceProvider;

                    try
                    {
                        var context = services.GetRequiredService<IHeroContext>();

                        var userManager = services.GetRequiredService<UserManager<AppUser>>();

                        var roleManager = services.GetRequiredService<RoleManager<AppRole>>();

                        await HeroContextSeedSeed.SeedAsync(context, userManager, roleManager);
                    }
                    catch (Exception ex)
                    {
                       // var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

                        logger.LogError(ex, "An error occurred while migrating or seeding the database.");
                    }
                }

                await host.RunAsync();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseSerilog();
    }
}
