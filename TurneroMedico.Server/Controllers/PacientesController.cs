using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TurneroMedico.Server.Data;
using TurneroMedico.Server.Models;

namespace TurneroMedico.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PacientesController : ControllerBase
    {
        private readonly TurneroContext _db;
        public PacientesController(TurneroContext db) => _db = db;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Paciente>>> GetAll() => await _db.Pacientes.ToListAsync();

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Paciente>> Get(int id)
        {
            var entity = await _db.Pacientes.FindAsync(id);
            return entity is null ? NotFound() : entity;
        }

        [HttpPost]
        public async Task<ActionResult<Paciente>> Create(Paciente paciente)
        {
            _db.Pacientes.Add(paciente);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = paciente.Id }, paciente);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, Paciente paciente)
        {
            if (id != paciente.Id) return BadRequest();
            _db.Entry(paciente).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _db.Pacientes.FindAsync(id);
            if (entity is null) return NotFound();
            _db.Pacientes.Remove(entity);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
