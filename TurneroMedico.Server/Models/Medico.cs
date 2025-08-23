using System.ComponentModel.DataAnnotations;

namespace TurneroMedico.Server.Models
{
    public class Medico : Persona
    {
        [MaxLength(50)]
        public string Matricula { get; set; } = string.Empty; // opcional: numero de matricula profesional

        // Un medico puede tener una o mas especialidades
        public List<Especialidad> Especialidades { get; set; } = new();
    }
}
