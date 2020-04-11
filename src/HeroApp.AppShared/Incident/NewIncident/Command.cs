using HeroApp.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HeroApp.AppShared.Incident.NewIncident
{
    public class Command : IRequest<ApiResponse<Result>>
    {
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
    }
}
