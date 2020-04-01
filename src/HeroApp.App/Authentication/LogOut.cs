
using HeroApp.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HeroApp.AppShared.Authentication.LogOut;

namespace HeroApp.App.Authentication
{
    public class LogOut
    {

      


        public class Handler : IRequestHandler<Command, ApiResponse>
        {
            private readonly SignInManager<AppUser> signInManager;

            public Handler( SignInManager<AppUser> signInManager)
            {
                this.signInManager = signInManager;
            }
            public async Task<ApiResponse> Handle(Command request, CancellationToken cancellationToken)
            {

                await signInManager.SignOutAsync();

                return ApiResponse.Success("Logout successfully!");
                
            }
        }
    }
}
