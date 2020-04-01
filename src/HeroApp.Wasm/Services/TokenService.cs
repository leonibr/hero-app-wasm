using Blazored.LocalStorage;
using HeroApp.AppShared.Model;
using HeroApp.AppShared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeroApp.Wasm.Services
{
    public class TokenService : ITokenService
    {
        private readonly ILocalStorageService storageService;

        public TokenService(ILocalStorageService storageService)
        {
            this.storageService = storageService;
        }
        public async Task<string> GetToken()
        {
            
                var local = await storageService.GetItemAsync<LocalUserInfo>(Constants.UserKey);
                return local?.AccessToken ?? null;
            
        }
        
    }
}
