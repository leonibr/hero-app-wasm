using HeroApp.Domain;
using HeroApp.Domain.Interfaces;

using HeroApp.Infra.Services;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Claims;


namespace HeroApp.Infra
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            services.AddDbContext<HeroContext>(options =>
                options.UseSqlite(
                    configuration.GetConnectionString("Sqlite"),
                    b => {
                        b.MigrationsHistoryTable(Constants.HistoryTableName);
                        b.MigrationsAssembly(typeof(HeroContext).Assembly.FullName);
                    }));


            services.AddDefaultIdentity<AppUser>()
                    .AddRoles<AppRole>()
                    .AddEntityFrameworkStores<HeroContext>();

            services.Configure<GzipCompressionProviderOptions>(options => options.Level = System.IO.Compression.CompressionLevel.Optimal);
            services.AddResponseCompression();
            services.AddScoped<IHeroContext>(provider => provider.GetService<HeroContext>());

            var tokenConfigurations = new TokenConfigurations();
            new ConfigureFromConfigurationOptions<TokenConfigurations>(
                configuration.GetSection("TokenConfigurations"))
                    .Configure(tokenConfigurations);

            services.AddSingleton(tokenConfigurations);
            var signingConfigurations = new SigningConfigurations(tokenConfigurations);

            services.AddSingleton(signingConfigurations);



                services.AddTransient<IDateTime, DateTimeService>();

                services.AddTransient<IIdentityService, IdentityService>();
           

           


            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(bearerOptions =>
            {

                //bearerOptions.ForwardDefaultSelector = ctx =>
                //{
                //    return ctx.Request.Path.StartsWithSegments("/api/report/escala-grupo") ? CookieAuthenticationDefaults.AuthenticationScheme : null;
                //};

                var paramsValidation = bearerOptions.TokenValidationParameters;
                paramsValidation.IssuerSigningKey = signingConfigurations.Key;
                paramsValidation.ValidAudience = tokenConfigurations.Audience;
                paramsValidation.ValidIssuer = tokenConfigurations.Issuer;
       
                paramsValidation.ValidateIssuerSigningKey = true;

                paramsValidation.ValidateLifetime = true;

              
                paramsValidation.ClockSkew = TimeSpan.Zero;
            });

            services.AddAuthorization(auth =>
            {
                var defaultPolicy = new AuthorizationPolicyBuilder(
                    JwtBearerDefaults.AuthenticationScheme
                
                    ).RequireAuthenticatedUser().Build();
                

                auth.DefaultPolicy = defaultPolicy;
            });

            return services;
        }
    }
}
