using HeroApp.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HeroApp.AppShared.Authentication.Authenticate
{
    public class Command : IRequest<ApiResponse<Result>>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Grant_Type => "password";
    }
}
