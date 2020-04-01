using HeroApp.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace HeroApp.AppShared.Authentication.LogOut
{
    public class Command : IRequest<ApiResponse>
    {
        public ClaimsPrincipal User { get; set; }
    }

}
