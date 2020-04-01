using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using HeroApp.Infra;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HeroApp.App.Profile
{    
    public class GetProfile
    {
            

        public class Handler : IRequestHandler<AppShared.Profile.GetProfile.Query, 
                                            IEnumerable<AppShared.Profile.GetProfile.Result>>
        {
            private readonly IHeroContext heroContext;
            private readonly IConfigurationProvider configProvider;

            public Handler(IHeroContext heroContext, IConfigurationProvider configProvider)
            {
                this.heroContext = heroContext;
                this.configProvider = configProvider;
            }
            public async Task<IEnumerable<AppShared.Profile.GetProfile.Result>> Handle(AppShared.Profile.GetProfile.Query request, CancellationToken cancellationToken)
            {
                var ongId = "AAAABBBB";
                var incidents = await heroContext
                   .Incidents
                   .Where(c => c.Ong_Id == ongId)
                   .ProjectTo<AppShared.Profile.GetProfile.Result>(configProvider)
                   .ToListAsync();

                return incidents;
            }
        }

    }
}
