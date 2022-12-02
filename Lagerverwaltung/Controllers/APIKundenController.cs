using Lagerverwaltung.Data;
using Lagerverwaltung.Filters;
using Lagerverwaltung.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace Lagerverwaltung.Controllers
{
    [Route("api/Kunden")]
    [ApiController]
    [ApiKeyAuthorization]
    public class APIKundenController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public APIKundenController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var all = _context.Kunde.ToArray();
            return Ok(all);
        }

        [HttpGet("GetById")] 
        public IActionResult GetById(int id) 
        { 
        
            var byId = _context.Kunde.SingleOrDefault(x => x.Id == id);

            if(byId == null)
            {
                return NotFound();
            }

            return Ok(byId);
        
        }

        [HttpPost("Create")]
        public IActionResult Create(Kunde kunde)
        {
            if (kunde.Id != 0)
            {
                return BadRequest();
            }

            _context.Kunde.Add(kunde);
            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete("Delete")]
        public IActionResult Delete(int id)
        {
            var byId = _context.Kunde.SingleOrDefault(x => x.Id == id);

            if (byId == null)
            {
                return NotFound();
            }

            _context.Kunde.Remove(byId);
            _context.SaveChanges();

            return Ok("Objekt wurde gelöscht!");
        }

        [HttpPut("Update")]
        public IActionResult Update(Kunde kunde)
        {
            if (kunde.Id == 0)
            {
                return BadRequest();
            }

            try
            {
                _context.Kunde.Update(kunde);
                _context.SaveChanges();
                return Ok("Die Änderungen wurden gespeichert.");
            }
            catch(Exception ex) 
            { 
                return BadRequest(ex.Message);
            }
            
        }
    }
}
