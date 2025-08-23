using System.ComponentModel.DataAnnotations;

namespace TurneroMedico.Server.Models
{
    public class Paciente : Persona
    {
        public TipoDocumento TipoDocumento { get; set; } = TipoDocumento.DNI;

        [MaxLength(32)]
        public string NumeroDocumento { get; set; } = string.Empty;

        [MaxLength(30)]
        public string Telefono { get; set; } = string.Empty;

        public DateOnly? FechaNacimiento { get; set; }

        [EmailAddress]
        [MaxLength(256)]
        public string Email { get; set; } = string.Empty;
    }
}
