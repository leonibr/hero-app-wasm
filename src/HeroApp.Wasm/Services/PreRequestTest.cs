using HeroApp.AppShared.Services;
using HeroApp.Wasm.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HeroApp.Wasm.Services
{
    public class PreRequestTest : IBeforeRequest
    {
        public PreRequestTest()
        {
            DebugPrint.Log("PreRequest test iniciado");
        
        }
        public Task Handle(HttpClient client)
        {
            // donadoa
            
            return Task.CompletedTask;
        }
    }
}
