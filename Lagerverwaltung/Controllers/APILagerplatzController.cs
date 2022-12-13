using Lagerverwaltung.Data;
using Lagerverwaltung.Filters;
using Lagerverwaltung.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace Lagerverwaltung.Controllers
{
    [Route("api/Lagerplatz")]
    [ApiController]
    [ApiKeyAuthorization]
    public class APILagerplatzController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public APILagerplatzController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var all = _context.Lagerplatz.ToArray();
            return Ok(all);
        }

        [HttpGet("GetById")] 
        public IActionResult GetById(int id) 
        { 
        
            var byId = _context.Lagerplatz.SingleOrDefault(x => x.Id == id);

            if(byId == null)
            {
                return NotFound();
            }

            return Ok(byId);
        
        }

        [HttpPost("Create")]
        public IActionResult Create(Lagerplatz Lagerplatz)
        {
            if (Lagerplatz.Id != 0)
            {
                return BadRequest();
            }

            try
            {
                _context.Lagerplatz.Add(Lagerplatz);
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("Delete")]
        public IActionResult Delete(int id)
        {
            var byId = _context.Lagerplatz.SingleOrDefault(x => x.Id == id);

            if (byId == null)
            {
                return NotFound();
            }

            _context.Lagerplatz.Remove(byId);
            _context.SaveChanges();

            return Ok("Objekt wurde gelöscht!");
        }

        [HttpPut("Update")]
        public IActionResult Update(Lagerplatz Lagerplatz)
        {
            if (Lagerplatz.Id == 0)
            {
                return BadRequest();
            }

            try
            {
                _context.Lagerplatz.Update(Lagerplatz);
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
