using Lagerverwaltung.Data;
using Lagerverwaltung.Filters;
using Lagerverwaltung.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace Lagerverwaltung.Controllers
{
    [Route("api/Position")]
    [ApiController]
    [ApiKeyAuthorization]
    public class APIPositionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public APIPositionController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var all = _context.Position.ToArray();
            return Ok(all);
        }

        [HttpGet("GetById")] 
        public IActionResult GetById(int id) 
        { 
        
            var byId = _context.Position.SingleOrDefault(x => x.Id == id);

            if(byId == null)
            {
                return NotFound();
            }

            return Ok(byId);
        
        }

        [HttpPost("Create")]
        public IActionResult Create(Positionen Position)
        {
            if (Position.Id != 0)
            {
                return BadRequest();
            }

            try
            {
                _context.Position.Add(Position);
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
            var byId = _context.Position.SingleOrDefault(x => x.Id == id);

            if (byId == null)
            {
                return NotFound();
            }

            _context.Position.Remove(byId);
            _context.SaveChanges();

            return Ok("Objekt wurde gelöscht!");
        }

        [HttpPut("Update")]
        public IActionResult Update(Positionen Position)
        {
            if (Position.Id == 0)
            {
                return BadRequest();
            }

            try
            {
                _context.Position.Update(Position);
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
