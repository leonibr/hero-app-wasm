using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using HeroApp.App.Services;
using HeroApp.AppShared.Ong.PostOng;
using HeroApp.Domain;
using HeroApp.Domain.Interfaces;
using HeroApp.Infra;
using MediatR;

namespace HeroApp.App.Ong
{
    public class PostOng
    {


        public class CommandValidator: AbstractValidator<Command>
        {
            private readonly IAuthUserService authUser;

            public CommandValidator(IAuthUserService authUser)
            {
                CascadeMode = CascadeMode.StopOnFirstFailure;
                this.authUser = authUser;

                RuleFor(c => c.Email).MustAsync(NotExistEmail).WithMessage("We already have your email. Click I forgot");

            }



            private async Task<bool> NotExistEmail(string userName, CancellationToken cancellationToken)
            {
                return !await authUser.UserExists(userName);
            }
        }


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
                var newUser = await authUser.GetUser(request.Email);
                var token = await authUser.GenerateToken(request.Email);
                var ong = mapper.Map<Domain.Ong>(request);
                string newId = generateIdService.GenerateId();
                ong.Id = newId;
                ong.Owner = newUser;
                heroContext.Ongs.Add(ong);
                return ApiResponse<Result>.SuccessFrom(new Result() { UserId = newUser.Id, AccessToken = token });        
                
            }
        }




    }
}
