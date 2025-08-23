using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TurneroMedico.Server.Data;
using TurneroMedico.Server.Models;

namespace TurneroMedico.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EspecialidadesController : ControllerBase
    {
        private readonly TurneroContext _db;
        public EspecialidadesController(TurneroContext db) => _db = db;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Especialidad>>> GetAll() => await _db.Especialidades.ToListAsync();

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Especialidad>> Get(int id)
        {
            var entity = await _db.Especialidades.FindAsync(id);
            return entity is null ? NotFound() : entity;
        }

        [HttpPost]
        public async Task<ActionResult<Especialidad>> Create(Especialidad especialidad)
        {
            _db.Especialidades.Add(especialidad);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = especialidad.Id }, especialidad);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, Especialidad especialidad)
        {
            if (id != especialidad.Id) return BadRequest();
            _db.Entry(especialidad).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _db.Especialidades.FindAsync(id);
            if (entity is null) return NotFound();
            _db.Especialidades.Remove(entity);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
