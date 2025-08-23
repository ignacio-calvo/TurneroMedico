using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TurneroMedico.Server.Data;
using TurneroMedico.Server.Models;

namespace TurneroMedico.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TurnosController : ControllerBase
    {
        private readonly TurneroContext _db;
        public TurnosController(TurneroContext db) => _db = db;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Turno>>> GetAll()
            => await _db.Turnos
                .Include(t => t.Medico)
                .Include(t => t.Paciente)
                .Include(t => t.Especialidad)
                .Include(t => t.ObraSocial)
                .ToListAsync();

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Turno>> Get(int id)
        {
            var entity = await _db.Turnos
                .Include(t => t.Medico)
                .Include(t => t.Paciente)
                .Include(t => t.Especialidad)
                .Include(t => t.ObraSocial)
                .FirstOrDefaultAsync(t => t.Id == id);
            return entity is null ? NotFound() : entity;
        }

        [HttpPost]
        public async Task<ActionResult<Turno>> Create(Turno turno)
        {
            // Validaciones básicas: existencia de claves y no solapamiento mismo médico/hora
            if (!await _db.Medicos.AnyAsync(m => m.Id == turno.MedicoId)) return BadRequest("Médico inválido");
            if (!await _db.Pacientes.AnyAsync(p => p.Id == turno.PacienteId)) return BadRequest("Paciente inválido");
            if (!await _db.Especialidades.AnyAsync(e => e.Id == turno.EspecialidadId)) return BadRequest("Especialidad inválida");
            if (!await _db.ObrasSociales.AnyAsync(o => o.Id == turno.ObraSocialId)) return BadRequest("Obra social inválida");

            var overlap = await _db.Turnos.AnyAsync(t => t.MedicoId == turno.MedicoId && t.FechaHora == turno.FechaHora && t.Estado != TurnoEstado.Cancelado);
            if (overlap) return Conflict("El médico ya tiene un turno en ese horario.");

            _db.Turnos.Add(turno);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = turno.Id }, turno);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, Turno turno)
        {
            if (id != turno.Id) return BadRequest();
            _db.Entry(turno).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _db.Turnos.FindAsync(id);
            if (entity is null) return NotFound();
            _db.Turnos.Remove(entity);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
