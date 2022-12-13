using Lagerverwaltung.Data;
using Lagerverwaltung.Filters;
using Lagerverwaltung.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace Lagerverwaltung.Controllers
{
    [Route("api/Auftrag")]
    [ApiController]
    [ApiKeyAuthorization]
    public class APIAuftragController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public APIAuftragController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var all = _context.Auftrag.ToArray();
            return Ok(all);
        }

        [HttpGet("GetById")] 
        public IActionResult GetById(int id) 
        { 
        
            var byId = _context.Auftrag.SingleOrDefault(x => x.Id == id);

            if(byId == null)
            {
                return NotFound();
            }

            return Ok(byId);
        
        }

        [HttpPost("Create")]
        public IActionResult Create(Auftrag Auftrag)
        {
            if (Auftrag.Id != 0)
            {
                return BadRequest();
            }

            try
            {
                _context.Auftrag.Add(Auftrag);
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
            var byId = _context.Auftrag.SingleOrDefault(x => x.Id == id);

            if (byId == null)
            {
                return NotFound();
            }

            _context.Auftrag.Remove(byId);
            _context.SaveChanges();

            return Ok("Objekt wurde gelöscht!");
        }

        [HttpPut("Update")]
        public IActionResult Update(Auftrag Auftrag)
        {
            if (Auftrag.Id == 0)
            {
                return BadRequest();
            }

            try
            {
                _context.Auftrag.Update(Auftrag);
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
