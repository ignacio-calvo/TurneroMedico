using Microsoft.EntityFrameworkCore;
using TurneroMedico.Server.Models;

namespace TurneroMedico.Server.Data
{
    public class TurneroContext : DbContext
    {
        public TurneroContext(DbContextOptions<TurneroContext> options) : base(options) { }

        public DbSet<Persona> Personas => Set<Persona>();
        public DbSet<Medico> Medicos => Set<Medico>();
        public DbSet<Paciente> Pacientes => Set<Paciente>();
        public DbSet<Especialidad> Especialidades => Set<Especialidad>();
        public DbSet<ObraSocial> ObrasSociales => Set<ObraSocial>();
        public DbSet<Turno> Turnos => Set<Turno>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // TPH para Persona/Medico/Paciente
            modelBuilder.Entity<Persona>()
                .HasDiscriminator<string>("Discriminator")
                .HasValue<Persona>("Persona")
                .HasValue<Medico>("Medico")
                .HasValue<Paciente>("Paciente");

            // Relacion many-to-many Medico-Especialidad
            modelBuilder.Entity<Medico>()
                .HasMany(m => m.Especialidades)
                .WithMany();

            // Turno relaciones
            modelBuilder.Entity<Turno>()
                .HasOne(t => t.Medico)
                .WithMany()
                .HasForeignKey(t => t.MedicoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Turno>()
                .HasOne(t => t.Paciente)
                .WithMany()
                .HasForeignKey(t => t.PacienteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Turno>()
                .HasOne(t => t.Especialidad)
                .WithMany()
                .HasForeignKey(t => t.EspecialidadId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Turno>()
                .HasOne(t => t.ObraSocial)
                .WithMany()
                .HasForeignKey(t => t.ObraSocialId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
