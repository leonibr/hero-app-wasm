using HeroApp.AppShared.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace HeroApp.Wasm.Services
{
    public class ClientSettings : IClientSettings
    {
        private readonly ClientSettings settings;

        public ClientSettings()
        {

        }
        public string BaseUrl { get; set; }
    }
}
