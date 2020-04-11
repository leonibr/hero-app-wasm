using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
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

namespace HeroApp.App.Profile
{    
    public class GetProfile
    {
            

        public class Handler : IRequestHandler<AppShared.Profile.GetProfile.Query, 
                                            ApiResponse<IEnumerable<AppShared.Profile.GetProfile.Result>>>
        {
            private readonly HeroContext heroContext;
            private readonly IMapper mapper;
            private readonly IConfigurationProvider configProvider;

            public Handler(HeroContext heroContext, IMapper mapper, IConfigurationProvider configProvider)
            {
                this.heroContext = heroContext;
                this.mapper = mapper;
                this.configProvider = configProvider;
            }
            public async Task<ApiResponse<IEnumerable<AppShared.Profile.GetProfile.Result>>> 
                Handle(AppShared.Profile.GetProfile.Query request, CancellationToken cancellationToken)
            {
                var owner = await heroContext.Users.Where(c => c.Id == request.UserId)
                    .Include(c => c.MyOngs)
                    .ThenInclude(c => c.Incidents)
                    //.Select(c => c.MyOngs.FirstOrDefault())
                    //.Select(c => c.Incidents)
                  //  .ProjectTo<IEnumerable<AppShared.Profile.GetProfile.Result>>(configProvider)
                    .FirstOrDefaultAsync();
                var incidents = owner.MyOngs.FirstOrDefault()
                    .Incidents
                    .Select(c => c);
                    

                var listResult = mapper.Map<IEnumerable<Domain.Incident>, IEnumerable<AppShared.Profile.GetProfile.Result>>(incidents);
               

                return ApiResponse<IEnumerable<AppShared.Profile.GetProfile.Result>>.SuccessFrom(listResult);
            }
        }

        public class Mapper: AutoMapper.Profile
        {
            public Mapper()
            {

                CreateMap<Domain.Incident, AppShared.Profile.GetProfile.Result>();
            }
        }

    }
}
