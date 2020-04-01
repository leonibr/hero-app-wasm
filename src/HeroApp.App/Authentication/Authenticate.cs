using FluentValidation;
using HeroApp.Domain;
using HeroApp.Infra;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HeroApp.AppShared.Authentication.Authenticate;

namespace HeroApp.App.Authentication
{
    public class Authenticate
    {





        public class Handler : IRequestHandler<Command, ApiResponse<Result>>
        {
            private readonly IHeroContext heroContext;
            private readonly IWebHostEnvironment env;
            private readonly SigningConfigurations signingConfigurations;
            private readonly UserManager<AppUser> userManager;
            private readonly RoleManager<AppRole> roleManager;
            private readonly SignInManager<AppUser> signInManager;
            private readonly TokenConfigurations tokenConfigurations;

            public Handler(IHeroContext heroContext,
                    IWebHostEnvironment env,
                    SigningConfigurations signingConfigurations,
                    UserManager<AppUser> userManager,
                    RoleManager<AppRole> roleManager,
                    SignInManager<AppUser> signInManager,
                    TokenConfigurations tokenConfigurations)
            {
                this.heroContext = heroContext;
                this.env = env;
                this.signingConfigurations = signingConfigurations;
                this.userManager = userManager;
                this.roleManager = roleManager;
                this.signInManager = signInManager;
                this.tokenConfigurations = tokenConfigurations;
            }
            public async Task<ApiResponse<Result>> Handle(Command request, CancellationToken cancellationToken)
            {



                var userName = request.Username;
                var userIdentity = await userManager.FindByNameAsync(userName);

                if (userIdentity == null)
                {

                    var newUser = new AppUser()
                    {
                        UserName = request.Username
                    };
                    
                    await userManager.CreateAsync(newUser);
                    var userProfile = await roleManager.FindByNameAsync(AppProfileConstants.USER);
                    await userManager.AddToRoleAsync(newUser, AppProfileConstants.USER);


                    userIdentity = await userManager.FindByNameAsync(request.Username);
                } else
                {
                    return ApiResponse<Result>.FailureFrom("User and/or passord invalid.");
                }


                var claims = new List<Claim>() {
                          new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                          new Claim(JwtRegisteredClaimNames.UniqueName, request.Username)
                        };


              
                await signInManager.SignInAsync(userIdentity,
                    new AuthenticationProperties()
                    {
                        AllowRefresh = false,
                        IsPersistent = false,
                        ExpiresUtc = DateTimeOffset.Now.AddSeconds(tokenConfigurations.Seconds)
                    });



                var roles = await userManager.GetRolesAsync(userIdentity);
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                ClaimsIdentity identity = new ClaimsIdentity(
                    claims
                );

                DateTime dataCriacao = DateTime.Now;
                DateTime dataExpiracao = dataCriacao +
                    TimeSpan.FromSeconds(tokenConfigurations.Seconds);

                var handler = new JwtSecurityTokenHandler();
                var securityToken = handler.CreateToken(new SecurityTokenDescriptor
                {
                    Issuer = tokenConfigurations.Issuer,
                    Audience = tokenConfigurations.Audience,
                    SigningCredentials = signingConfigurations.SigningCredentials,
                    Subject = identity,
                    NotBefore = dataCriacao,
                    Expires = dataExpiracao

                });
                var token = handler.WriteToken(securityToken);



                var result = new Result()
                {
                    Authenticated = true,
                    Created = dataCriacao.ToString("yyyy-MM-dd HH:mm:ss"),
                    Expiration = dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss"),
                    AccessToken = token,
                    UserName = request.Username,
                    Message = "OK",
                };
                return ApiResponse<Result>.SuccessFrom(result);

            }
              
        }
    }


}
