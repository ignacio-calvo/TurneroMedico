using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TurneroMedico.Server.Data;
using TurneroMedico.Server.Models;

namespace TurneroMedico.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonasController : ControllerBase
    {
        private readonly TurneroContext _db;
        public PersonasController(TurneroContext db) => _db = db;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Persona>>> GetAll() => await _db.Personas.ToListAsync();

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Persona>> Get(int id)
        {
            var entity = await _db.Personas.FindAsync(id);
            return entity is null ? NotFound() : entity;
        }

        [HttpPost]
        public async Task<ActionResult<Persona>> Create(Persona persona)
        {
            _db.Personas.Add(persona);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = persona.Id }, persona);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, Persona persona)
        {
            if (id != persona.Id) return BadRequest();
            _db.Entry(persona).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _db.Personas.FindAsync(id);
            if (entity is null) return NotFound();
            _db.Personas.Remove(entity);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
