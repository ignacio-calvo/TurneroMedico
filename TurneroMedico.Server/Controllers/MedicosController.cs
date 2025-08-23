using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TurneroMedico.Server.Data;
using TurneroMedico.Server.Models;

namespace TurneroMedico.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicosController : ControllerBase
    {
        private readonly TurneroContext _db;
        public MedicosController(TurneroContext db) => _db = db;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Medico>>> GetAll() => await _db.Medicos.Include(m => m.Especialidades).ToListAsync();

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Medico>> Get(int id)
        {
            var entity = await _db.Medicos.Include(m => m.Especialidades).FirstOrDefaultAsync(m => m.Id == id);
            return entity is null ? NotFound() : entity;
        }

        [HttpPost]
        public async Task<ActionResult<Medico>> Create(Medico medico)
        {
            _db.Medicos.Add(medico);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = medico.Id }, medico);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, Medico medico)
        {
            if (id != medico.Id) return BadRequest();
            _db.Entry(medico).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _db.Medicos.FindAsync(id);
            if (entity is null) return NotFound();
            _db.Medicos.Remove(entity);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
