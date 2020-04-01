using HeroApp.AppShared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeroApp.Wasm.Services
{
    public class BaseUrlService : IBaseUrlService
    {
        public BaseUrlService()
        {
            Console.WriteLine("BaseUrlService: Initialized");
        }
        public Uri Uri => new Uri("https://localhost:5001");
    }
}
