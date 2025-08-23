using Microsoft.EntityFrameworkCore;
using TurneroMedico.Server.Models;

namespace TurneroMedico.Server.Data
{
    public static class SeedData
    {
        public static void Initialize(TurneroContext db)
        {
            // Ensure database created (InMemory no-op, but keeps parity)
            db.Database.EnsureCreated();

            // Seed Especialidades
            if (!db.Especialidades.Any())
            {
                var especialidades = new[]
                {
                    "Cardiología",
                    "Cirugía Infantil",
                    "Kinesiología",
                    "Nefrología",
                    "Nutrición Infantil",
                    "Pediatría",
                    "Traumatología",
                };

                foreach (var nombre in especialidades)
                    db.Especialidades.Add(new Especialidad { Nombre = nombre });

                db.SaveChanges();
            }

            // Seed Obras Sociales
            if (!db.ObrasSociales.Any())
            {
                var obras = new[]
                {
                    "AG.PROPAGANDA MEDICA (A.P.M.)",
                    "AMS",
                    "ASOCIACION MUTUAL SANCOR",
                    "AVALIAN",
                    "BOREAL COBERTURA DE SALUD S.A.",
                    "COLEGIO ESCRIBANOS",
                    "CONS.PROF.CS.ECONOMICAS",
                    "DOCTHOS",
                    "DOCTHOS PREMIUM",
                    "EMPLADOS BCO.PCIA BS.AS.",
                    "FARMACIAS",
                    "FUTBOLISTA",
                    "G.C.G SALUD",
                    "GALENO ARGENTINO",
                    "GALENO SALUD",
                    "I.P.S.",
                    "I.P.S. ACC.ESCOLAR",
                    "IOSFA",
                    "IPSS Accidente Escolar",
                    "JERARQUICOS SALUD",
                    "LUZ Y FUERZA",
                    "MEDICAL GROUP",
                    "MEDICUS (E 088998)",
                    "MEDIFE",
                    "MEPLIFE SALUD",
                    "NOBIS SA",
                    "O.S De Maquinista de teatro TV",
                    "O.S.D.I.P.P",
                    "O.S.D.I.P.P.",
                    "O.S.P.E.S.G.A",
                    "OBRA SOCIAL DE CERAMISTA",
                    "OMINT",
                    "OSAM MADERERO INTEGRAL SRL",
                    "OSAM MADEREROS CONV. INTEGRAL",
                    "OSDE BINARIO - PLAN 2-210",
                    "OSDE BINARIO - PLAN 2-310",
                    "OSDE BINARIO - PLAN 2-410",
                    "OSDEPYM",
                    "OSDIPP",
                    "OSDOP",
                    "OSEIV O.S.DEL VIDRIO",
                    "OSFATLYF",
                    "OSMATA",
                    "OSPACARP",
                    "OSPATCA",
                    "OSPE",
                    "OSPEDYC",
                    "OSPESGA",
                    "OSPIDA",
                    "OSPIF",
                    "OSPIL",
                    "OSPSA",
                    "OSSEG",
                    "OSTEL",
                    "OSUTHGRA",
                    "PARTICULAR",
                    "PERSONAL DE PANADERIAS",
                    "PODER JUDICIAL",
                    "PREVENCION SALUD",
                    "RED DE SEGURO MEDICO SRL",
                    "RIOMEDICA SA",
                    "S.A.T. (Televisión)",
                    "S.P.M. GALENO ARGENTINA",
                    "SADAIC",
                    "SCIS",
                    "SCIS MEDICINA PRIVADA",
                    "SERVICIO PENITENCIARIO",
                    "SIN COBERTURA",
                    "SPF",
                    "SWISS MEDICAL GROUP",
                    "U.N.S.A.",
                    "UNION PERSONAL",
                    "VETERANOS DE GUERRA",
                    "VISITAR",
                    "VISITAR ANDAR",
                };

                foreach (var nombre in obras)
                    db.ObrasSociales.Add(new ObraSocial { Nombre = nombre });

                db.SaveChanges();
            }

            // Seed Medicos (al menos 3 por especialidad)
            var todasEsp = db.Especialidades.AsNoTracking().ToList();

            var nombres = new[] { "Ana", "Carlos", "Lucía", "Javier", "María", "Pablo", "Sofía", "Diego", "Laura", "Nicolás" };
            var apellidos = new[] { "García", "Pérez", "Rodríguez", "Sánchez", "López", "Gómez", "Díaz", "Martínez", "Fernández", "Romero" };

            int nameIdx = 0;
            foreach (var esp in todasEsp)
            {
                var existentes = db.Medicos.Include(m => m.Especialidades).ToList()
                    .Count(m => m.Especialidades.Any(e => e.Id == esp.Id));

                var faltan = Math.Max(0, 3 - existentes);
                if (faltan <= 0) continue;

                // Attach tracked especialidad
                var espTracked = db.Especialidades.First(e => e.Id == esp.Id);

                for (int i = 0; i < faltan; i++)
                {
                    var nombre = nombres[nameIdx % nombres.Length];
                    var apellido = apellidos[nameIdx % apellidos.Length];
                    nameIdx++;

                    var medico = new Medico
                    {
                        Nombre = nombre,
                        Apellido = apellido,
                        Matricula = $"{esp.Id:D3}-{i + 1:D3}",
                        Especialidades = new List<Especialidad> { espTracked }
                    };

                    db.Medicos.Add(medico);
                }

                db.SaveChanges();
            }
        }
    }
}
