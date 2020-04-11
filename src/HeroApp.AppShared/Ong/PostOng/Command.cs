using HeroApp.AppShared.Common;
using HeroApp.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HeroApp.AppShared.Ong.PostOng
{
    public class Command: IRequest<ApiResponse<Result>>, IMapFrom<Domain.Ong>
    {
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string WhatsApp { get; set; }
        public string City { get; set; }
        public string Uf { get; set; }
    }
}
