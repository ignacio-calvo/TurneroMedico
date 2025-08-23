using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TurneroMedico.Server.Data;
using TurneroMedico.Server.Models;

namespace TurneroMedico.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ObrasSocialesController : ControllerBase
    {
        private readonly TurneroContext _db;
        public ObrasSocialesController(TurneroContext db) => _db = db;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ObraSocial>>> GetAll() => await _db.ObrasSociales.ToListAsync();

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ObraSocial>> Get(int id)
        {
            var entity = await _db.ObrasSociales.FindAsync(id);
            return entity is null ? NotFound() : entity;
        }

        [HttpPost]
        public async Task<ActionResult<ObraSocial>> Create(ObraSocial obra)
        {
            _db.ObrasSociales.Add(obra);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = obra.Id }, obra);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, ObraSocial obra)
        {
            if (id != obra.Id) return BadRequest();
            _db.Entry(obra).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _db.ObrasSociales.FindAsync(id);
            if (entity is null) return NotFound();
            _db.ObrasSociales.Remove(entity);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
