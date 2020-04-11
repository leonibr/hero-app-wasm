using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HeroApp.Domain;
using HeroApp.Domain.Interfaces;
using HeroApp.Infra;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HeroApp.Api.Controllers
{
    [Authorize]
    public class ProfileController : HeroBaseController
    {



        public ProfileController()
        {

        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<AppShared.Profile.GetProfile.Result>>>>
            GetProfile()
        {

            var userId = CurrentUserService.UserId;

            var query = new AppShared.Profile.GetProfile.Query()
            {
                UserId = userId
            };
            var incidents = await Mediator.Send(query);

            return Ok(incidents);
        }


    }



}