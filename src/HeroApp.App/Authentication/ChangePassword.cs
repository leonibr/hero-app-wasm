using FluentValidation;
using HeroApp.Domain;
using HeroApp.Infra;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HeroApp.AppShared.Authentication.ChangePassword;

namespace HeroApp.App.Authentication
{
    public class ChangePassword
    {

        public class Handler : IRequestHandler<Command, ApiResponse<Result>>
        {
            private readonly IHeroContext heroContext;
            private readonly UserManager<AppUser> userManager;
            private readonly SignInManager<AppUser> signInManager;
            private readonly TokenConfigurations tokenConfigurations;

            public Handler(IHeroContext heroContext,
                    UserManager<AppUser> userManager,
                    SignInManager<AppUser> signInManager,
                    TokenConfigurations tokenConfigurations)
            {
                this.heroContext = heroContext;
                this.userManager = userManager;
                this.signInManager = signInManager;
                this.tokenConfigurations = tokenConfigurations;
            }
            public Task<ApiResponse<Result>> Handle(Command request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}
