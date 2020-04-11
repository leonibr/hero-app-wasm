using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Blazored.LocalStorage;
using HeroApp.Wasm.Services;
using Microsoft.AspNetCore.Components.Authorization;
using HeroApp.AppShared.Services;
using Radzen;
using PeterLeslieMorris.Blazor.Validation;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using AutoMapper;
using System.Reflection;

namespace HeroApp.Wasm
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<DialogService>();
            builder.Services.AddScoped<NotificationService>();
            builder.Services.AddScoped<IBaseUrlService, BaseUrlService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
          
            // builder.Services.AddHttpClientService(c =>
            // {
            //     c.TimeOut = 5000;
            //     c.AddBeforeRequest<PreRequestTest>();
            //     //c.AddAfterRequest<UnauthorizedResponse>();

            // });
            builder.Services.AddBlazoredLocalStorage();
            //builder.Services.AddFormValidation(config => config.AddDataAnnotationsValidation());
            builder.Services.AddFormValidation(config => config.AddFluentValidation(typeof(AppShared.Class1).Assembly));
            builder.Services.AddOptions();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<AuthenticationStateProvider, LocalAuthenticationStateProvider>();
            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly(), Assembly.GetAssembly(typeof(AppShared.Class1)));
           


            builder.RootComponents.Add<App>("app");


            await builder.Build()
                .UseLocalTimeZone()
                .RunAsync();
        }
    }
}
