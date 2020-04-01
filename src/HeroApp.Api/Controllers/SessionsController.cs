using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HeroApp.Infra;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HeroApp.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SessionsController : ControllerBase
    {
        private readonly IHeroContext heroContext;

        public SessionsController(IHeroContext heroContext)
        {
            this.heroContext = heroContext;
        }


        [HttpPost]
        public async Task<ActionResult<PostSessionResult>> PostSession(PostSessionCommand command)
        {
            var result = await heroContext.Ongs
                    .Where(c => c.Id == command.Id)
                    .Select(c => new PostSessionResult()
                    {
                        Name = c.Name
                    })
                    .FirstOrDefaultAsync();
            if (result == null)
            {
                return BadRequest(new
                {
                    Error = "No ONG found with this id"
                });
            }

            return Ok(result);
        }
    }

    public class PostSessionResult
    {
        public string Name { get; set; }
    }

    public class PostSessionCommand
    {
        public string Id { get; set; }
    }

}