using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using HeroApp.Infra;
using HeroApp.Domain;
using Microsoft.AspNetCore.Authorization;

namespace HeroApp.Api.Controllers
{
   
    public class IncidentsController : HeroBaseController
    {
        private readonly IHeroContext _context;

        public IncidentsController(IHeroContext context)
        {
            _context = context;
        }

        


        // GET: api/Incidents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetIncidentsQuery>>> GetIncidents()
        {
            var page = Request.Query["page"].Count == 0 ? new StringValues("1") : Request.Query["page"];
            int.TryParse(page.ToString(), out int intPage);
            if (intPage == 0)
            {
                return NotFound();
            }

            if (CurrentUserService.UserId == string.Empty)
            {
                return NotFound();
            }

            var result = await _context.Incidents
                .Include(c => c.Ong)
                .Where(c => c.Ong_Id == CurrentUserService.UserId.ToString())
                .Take(5)
                .Skip((intPage - 1) * 5)
                .AsNoTracking()
                .Select(inc => new GetIncidentsQuery
                {
                    Id = inc.Id,
                    Title = inc.Title,
                    Description = inc.Description,
                    Value = inc.Value,
                    Name = inc.Ong.Name,
                    Email = inc.Ong.Email,
                    WhatsApp = inc.Ong.WhatsApp,
                    City = inc.Ong.City,
                    Uf = inc.Ong.State
                })
                .ToListAsync();

            Response.Headers["X-Total-Count"] = new StringValues(result.Count().ToString());

            return result;
        }


    
        // GET: api/Incidents/5
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Incident>> GetIncident(long id)
        {
            var incident = await _context.Incidents.FindAsync(id);

            if (incident == null)
            {
                return NotFound();
            }

            return incident;
        }





        // POST: api/Incidents
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ApiResponse<AppShared.Incident.NewIncident.Result>>>
            PostIncident([FromBody] AppShared.Incident.NewIncident.Command command)
        {

            if (CurrentUserService.UserId == string.Empty)
            {
                return Unauthorized();
            }
            command.UserId = CurrentUserService.UserId;
            var response =await Mediator.Send(command);

            return CreatedAtAction("PostIncident", response);
        }




        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<AppShared.Incident.Delete.Result>>>
    DeleteIncident(AppShared.Incident.Delete.Query query)
        {
            var response = await Mediator.Send(query);
            if (!response.Succeeded)
            {
                return NotFound(response);
            }
            return Ok(response);
        }


    }



    public class GetIncidentsQuery
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
        public string Ong_Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string WhatsApp { get; set; }
        public string City { get; set; }
        public string Uf { get; set; }
    }
}
