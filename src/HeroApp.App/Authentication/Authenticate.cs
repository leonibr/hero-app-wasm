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
using HeroApp.AppShared.Authentication.Login;
using HeroApp.App.Services;

namespace HeroApp.App.Authentication
{
    public class Login
    {





        public class Handler : IRequestHandler<Command, ApiResponse<Result>>
        {
            private readonly IAuthUserService userService;

            public Handler(IAuthUserService userService)
            {


                this.userService = userService;
            }
            public async Task<ApiResponse<Result>> Handle(Command request, CancellationToken cancellationToken)
            {
                AppUser user;
                if (!await userService.UserExists(request.Email))
                {
                   var resultr = await userService.CreateMinimunUser(userName: request.Email, password: request.Password);
                   
                    if (!resultr.Succeeded)
                    {
                        return ApiResponse<Result>.FailureFrom(resultr.Errors.Select(c => c.Description));

                    }
                }
                else
                {
                    return ApiResponse<Result>.FailureFrom("User already exists, retrive your access code by sending email to contact@heroapp.com");
                }
                user = await userService.GetUser(request.Email);
                string token = await userService.GenerateToken(userName: request.Email);

                var result = new Result()
                {
                    Authenticated = true,
                    AccessToken = token,
                    UserId = user.Id,
                    Message = "OK",
                };
                return ApiResponse<Result>.SuccessFrom(result);

            }

        }





    }


}
