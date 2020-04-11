using HeroApp.AppShared.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HeroApp.Wasm.Services
{
    public class UnauthorizedResponse : IAfterRequest
    {
        private readonly NavigationManager navigationManager;
        private readonly ITokenService tokenService;

        public UnauthorizedResponse(NavigationManager navigationManager, ITokenService tokenService)
        {
            this.navigationManager = navigationManager;
            this.tokenService = tokenService;
        }
        public async Task<HttpResponseMessage> Handle(HttpResponseMessage responseMessage)
        {
           if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
               await tokenService.Clear();
                navigationManager.NavigateTo("/");
                return null;
            }

            return responseMessage;
        }
    }
}
