using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HeroApp.Domain;
using HeroApp.Infra;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



namespace HeroApp.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OngsController : HeroBaseController
    {
        private readonly IHeroContext _context;

        public OngsController(IHeroContext context)
        {
            _context = context;
        }

        // GET: ongs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetOngResult>>> GetOngs()
        {
            return await _context.Ongs
                .AsNoTracking()
                .Select(c => new GetOngResult
                {
                    Id = c.Id,
                    Name = c.Name,
                    Email = c.Email,
                    WhatsApp = c.WhatsApp,
                    City = c.City,
                    Uf = c.State
                })
                .ToListAsync();
            
        }

        // GET: ongs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ong>> GetOng(string id)
        {
            var ong = await _context.Ongs.FindAsync(id);

            if (ong == null)
            {
                return NotFound();
            }

            return ong;
        }

        /* PUT: ongs/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOng(string id, Ong ong)
        {
            if (id != ong.Id)
            {
                return BadRequest();
            }

            _context.Entry(ong).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OngExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        */

        // POST: ongs
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Ong>> PostOng(
            [FromBody]
            AppShared.Ong.PostOng.Command command)
        {
            var result = await Mediator.Send(command);

            return CreatedAtAction("PostOng", result);
        }
        /*
        // DELETE: ongs/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Ong>> DeleteOng(string id)
        {
            var ong = await _context.Ongs.FindAsync(id);
            if (ong == null)
            {
                return NotFound();
            }

            _context.Ongs.Remove(ong);
            await _context.SaveChangesAsync();

            return ong;
        }*/

        private bool OngExists(string id)
        {
            return _context.Ongs.Any(e => e.Id == id);
        }
    }

    public class GetOngResult
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string WhatsApp { get; set; }
        public string City { get; set; }
        public string Uf { get; set; }
    }

 

}
