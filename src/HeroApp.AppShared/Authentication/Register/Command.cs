using HeroApp.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace HeroApp.AppShared.Authentication.Register
{
    
    public class Command: IRequest<ApiResponse<Result>>
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public string Email { get; set; }
        public string City { get; set; }
        public string Whatsapp { get; set; }
        public string State { get; set; }

    }
}
