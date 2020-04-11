using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using HeroApp.AppShared.Incident.NewIncident;
using HeroApp.Domain;
using System.Threading.Tasks;
using System.Threading;
using HeroApp.App.Services;
using HeroApp.Infra;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace HeroApp.App.Incident
{
    public class NewIncident
    {
        public class Handler : IRequestHandler<Command, ApiResponse<Result>>
        {
            
            private readonly HeroContext heroContext;
            private readonly IMapper mapper;

            public Handler(HeroContext heroContext, IMapper mapper)
            {
            
                this.heroContext = heroContext;
                this.mapper = mapper;
            }
            public async Task<ApiResponse<Result>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await heroContext.Users.Where(u => u.Id == request.UserId)
                            .Include(inc => inc.MyOngs)
                            .FirstOrDefaultAsync();

                if (user == null) return ApiResponse<Result>.FailureFrom("User not found");

                var ong = user.MyOngs.FirstOrDefault();

                var incident = mapper.Map<Domain.Incident>(request);

                incident.Ong = ong;

                heroContext.Add(incident);

                return ApiResponse<Result>.SuccessFrom(new Result()
                {
                    Id = incident.Id,                    
                });
            
            }
        }


        public class Mapper : AutoMapper.Profile
        {
            public Mapper()
            {
                CreateMap<Command, Domain.Incident>();
            }
        }


    }
}
