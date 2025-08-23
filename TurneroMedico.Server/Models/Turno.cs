using System.ComponentModel.DataAnnotations;

namespace TurneroMedico.Server.Models
{
    public class Turno
    {
        public int Id { get; set; }

        // Asociaciones claves
        public int MedicoId { get; set; }
        public Medico? Medico { get; set; }

        public int EspecialidadId { get; set; }
        public Especialidad? Especialidad { get; set; }

        public int PacienteId { get; set; }
        public Paciente? Paciente { get; set; }

        public int ObraSocialId { get; set; }
        public ObraSocial? ObraSocial { get; set; }

        // Fecha y hora del turno
        [Required]
        public DateTime FechaHora { get; set; }

        // Observaciones opcionales
        [MaxLength(500)]
        public string Observaciones { get; set; } = string.Empty;

        // Estado del turno (opcional, simple por ahora)
        public TurnoEstado Estado { get; set; } = TurnoEstado.Reservado;
    }

    public enum TurnoEstado
    {
        Reservado,
        Confirmado,
        Cancelado,
        Ausente,
        Atendido
    }
}
