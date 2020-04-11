using Blazored.LocalStorage;
using HeroApp.AppShared.Model;
using HeroApp.AppShared.Services;
using HeroApp.Wasm.Shared;
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

        public async Task Clear()
        {
            await storageService.RemoveItemAsync(Constants.UserKey);
        }

        public async Task StoreToken(LocalUserInfo localUserInfo)
        {
            try
            {
                await storageService.SetItemAsync(Constants.UserKey, localUserInfo);
            }
            catch (Exception ex)
            {
                DebugPrint.Log("StoreToken");
                DebugPrint.Log(ex.Message);

                await storageService.ClearAsync();
            }
        }
    }
}
