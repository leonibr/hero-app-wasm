using HeroApp.AppShared.Services;
using HeroApp.Wasm.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeroApp.Wasm.Services
{
    public class BaseUrlService : IBaseUrlService
    {
        
        private string url;

        public BaseUrlService(IClientSettings clientSettings)
        {
            this.url = clientSettings.BaseUrl ?? "http://broken-home.com";
            DebugPrint.Log($"BaseUrlService initializaed with {url}");
        }
        public Uri Uri => new Uri(url);
    }
}
