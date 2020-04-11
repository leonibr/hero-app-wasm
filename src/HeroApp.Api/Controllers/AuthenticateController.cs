using HeroApp.Domain;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeroApp.Api.Controllers
{

    public class AuthenticateController : HeroBaseController
    {

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<AppShared.Authentication.Login.Result>>>
            PostLogin(
            [FromBody]
            AppShared.Authentication.Login.Command command)
        {
            var result = await Mediator.Send(command);

            return Ok(result);
        }

        [HttpPost("register")]
      
        public async Task<ActionResult<ApiResponse<AppShared.Authentication.Register.Result>>>
    PostLogin(
    [FromBody]
            AppShared.Authentication.Register.Command command)
        {
            var result = await Mediator.Send(command);

            return Ok(result);
        }
    }

}
