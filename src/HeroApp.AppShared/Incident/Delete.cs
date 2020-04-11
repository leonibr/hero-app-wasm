using HeroApp.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace HeroApp.AppShared.Incident
{
 public class Delete
    {
        public class Query : IRequest<ApiResponse<Result>>
        {
            [FromRoute]
            public long Id { get; set; }
        }






        public class Result
        {
            public bool IsSuccess { get; set; }
        }
    }
}
