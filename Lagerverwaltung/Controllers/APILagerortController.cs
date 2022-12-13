using Lagerverwaltung.Data;
using Lagerverwaltung.Filters;
using Lagerverwaltung.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace Lagerverwaltung.Controllers
{
    [Route("api/Lagerort")]
    [ApiController]
    [ApiKeyAuthorization]
    public class APILagerortController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public APILagerortController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var all = _context.Lagerort.ToArray();
            return Ok(all);
        }

        [HttpGet("GetById")] 
        public IActionResult GetById(int id) 
        { 
        
            var byId = _context.Lagerort.SingleOrDefault(x => x.Id == id);

            if(byId == null)
            {
                return NotFound();
            }

            return Ok(byId);
        
        }

        [HttpPost("Create")]
        public IActionResult Create(Lagerort Lagerort)
        {
            if (Lagerort.Id != 0)
            {
                return BadRequest();
            }

            try
            {
                _context.Lagerort.Add(Lagerort);
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
            var byId = _context.Lagerort.SingleOrDefault(x => x.Id == id);

            if (byId == null)
            {
                return NotFound();
            }

            _context.Lagerort.Remove(byId);
            _context.SaveChanges();

            return Ok("Objekt wurde gelöscht!");
        }

        [HttpPut("Update")]
        public IActionResult Update(Lagerort Lagerort)
        {
            if (Lagerort.Id == 0)
            {
                return BadRequest();
            }

            try
            {
                _context.Lagerort.Update(Lagerort);
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
