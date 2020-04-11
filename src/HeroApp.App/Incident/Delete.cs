using HeroApp.Domain;
using HeroApp.Infra;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HeroApp.App.Incident
{
    public class Delete
    {
        public class Handler : IRequestHandler<AppShared.Incident.Delete.Query, ApiResponse<AppShared.Incident.Delete.Result>>
        {
            private readonly IHeroContext heroContext;

            public Handler(IHeroContext heroContext)
            {
                this.heroContext = heroContext;
            }
            public async Task<ApiResponse<AppShared.Incident.Delete.Result>> Handle(AppShared.Incident.Delete.Query request, CancellationToken cancellationToken)
            {
                var item = await heroContext.Incidents.Where(c => c.Id == request.Id).FirstOrDefaultAsync();
                if (item == null) return ApiResponse<AppShared.Incident.Delete.Result>.FailureFrom("Incident not found");

                heroContext.Incidents.Remove(item);

                return ApiResponse<AppShared.Incident.Delete.Result>.SuccessFrom(new AppShared.Incident.Delete.Result()
                {
                    IsSuccess = true
                });
            }
        }
    }
}
