using AutoMapper;
using HeroApp.App.Services;
using HeroApp.AppShared.Authentication.Register;
using HeroApp.Domain;
using HeroApp.Domain.Interfaces;
using HeroApp.Infra;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HeroApp.App.Authentication
{
    public class Register
    {

        public class Handler : IRequestHandler<Command, ApiResponse<Result>>
        {
            private readonly IMapper mapper;
            private readonly IHeroContext heroContext;
            private readonly IGenerateIdService generateIdService;
            private readonly IAuthUserService authUser;

            public Handler(IMapper mapper, IHeroContext heroContext, IGenerateIdService generateIdService, IAuthUserService authUser)
            {
                this.mapper = mapper;
                this.heroContext = heroContext;
                this.generateIdService = generateIdService;
                this.authUser = authUser;
            }
            public async Task<ApiResponse<Result>> Handle(Command request, CancellationToken cancellationToken)
            {
                var identityResult = await authUser.CreateMinimunUser(request.Email, request.Password);
                if (!identityResult.Succeeded) return ApiResponse<Result>.FailureFrom(identityResult.Errors.Select(c => c.Description));
                var user = await authUser.GetUser(request.Email);
                var token = await authUser.GenerateToken(request.Email);
                var ong = mapper.Map<Domain.Ong>(request);
                string newId = generateIdService.GenerateId();
                ong.Id = newId;
                ong.Owner = user;
                heroContext.Ongs.Add(ong);
                return ApiResponse<Result>.SuccessFrom(new Result() { UserId = user.Id, AccessToken = token });

            }
        }


        public class Mapper: AutoMapper.Profile
        {
            public Mapper()
            {
                CreateMap<Command, Domain.Ong>();
            }
        }
    }
}
