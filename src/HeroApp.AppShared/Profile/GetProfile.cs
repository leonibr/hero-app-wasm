using FluentValidation;
using HeroApp.AppShared.Common;
using HeroApp.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace HeroApp.AppShared.Profile
{
    public class GetProfile
    {
        public class Query: IRequest<ApiResponse<IEnumerable<AppShared.Profile.GetProfile.Result>>>
        {
            
            public string UserId { get; set; }
        }

        public class QueryValidation: AbstractValidator<Query>
        {
            public QueryValidation()
            {
                RuleFor(q => q.UserId).NotEmpty();
            }

        }

        public class Result
        {
            public long Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Value { get; set; }
            public object Ong_Id { get; set; }
        }  


        public class Mapper: AutoMapper.Profile
        {
            public Mapper()
            {
                CreateMap<Domain.Incident, Result>().ReverseMap();
            }
        }

    
    }
}
