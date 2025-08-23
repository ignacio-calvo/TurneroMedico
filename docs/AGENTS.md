# Guía para agentes

Este documento ofrece a agentes de IA y a desarrolladores el contexto necesario para trabajar en toda la solución.

## Resumen
Turnero Médico es un sistema simple de turnos:
- Gestión de Personas, Médicos, Pacientes, Especialidades, Obras Sociales y Turnos.
- ASP.NET Core Web API (.NET 9, C# 13) con EF Core (InMemory para desarrollo).
- Front-end React + Vite (turneromedico.client).

## Stack
- Runtime: .NET 9, C# 13, ASP.NET Core Web API
- Datos: Entity Framework Core InMemory (desarrollo)
- OpenAPI: builder.Services.AddOpenApi(); app.MapOpenApi() en Development
- Cliente: React + TypeScript + Vite

## Estructura de la solución
- TurneroMedico.Server: Web API, controllers, DbContext, modelos de dominio
- TurneroMedico.AppHost: Aspire AppHost para orquestar la app
- TurneroMedico.ServiceDefaults: configuraciones de hosting compartidas
- turneromedico.client: front-end React

## Modelo de dominio (simplificado)
- Persona { Id, Nombre, Apellido }
  - Medico : Persona { Matricula, Especialidades: List<Especialidad> }
  - Paciente : Persona { TipoDocumento, NumeroDocumento, Telefono, FechaNacimiento, Email }
- Especialidad { Id, Nombre }
- ObraSocial { Id, Nombre }
- Turno { Id, MedicoId, EspecialidadId, PacienteId, ObraSocialId, FechaHora, Observaciones, Estado }

Notas:
- EF Core usa herencia TPH para Persona/Medico/Paciente.
- Turno tiene relaciones requeridas con Médico, Paciente, Especialidad y ObraSocial.

## Endpoints de API (dev)
URLs base (ver Properties/launchSettings.json):
- https: https://localhost:7120
- http:  http://localhost:5110

Rutas principales:
- GET/POST/PUT/DELETE /api/Personas
- GET/POST/PUT/DELETE /api/Medicos
- GET/POST/PUT/DELETE /api/Pacientes
- GET/POST/PUT/DELETE /api/Especialidades
- GET/POST/PUT/DELETE /api/ObrasSociales
- GET/POST/PUT/DELETE /api/Turnos

OpenAPI (solo Development):
- JSON: /openapi/v1.json

Prueba rápida (archivo .http de VS): TurneroMedico.Server/TurneroMedico.Server.http

## Datos de seed
Se ejecuta al iniciar (Program.cs -> SeedData.Initialize):
- Especialidades (Camel/Title Case): Cardiología, Cirugía Infantil, Kinesiología, Nefrología, Nutrición Infantil, Pediatría, Traumatología
- Obras sociales: lista completa provista en requerimientos
- Médicos: al menos 3 por especialidad con nombres de ejemplo y matrícula

## Convenciones
- C#: nullable habilitado, propiedades en PascalCase, un tipo por archivo
- Endpoints: RESTful, sustantivos en plural (api/Medicos, api/Pacientes, …)
- Validación: usar data annotations cuando aplique; evitar solapamiento de Turnos por Médico+FechaHora
- Ramas: feature/*, bugfix/*, hotfix/* (desde development)
- Commits: concisos, en imperativo (ej.: "Add Turnos controller")

## Lineamientos de cambio
- Nueva entidad: crear modelo, DbSet en TurneroContext, controller CRUD, actualizar seed si corresponde
- Nuevo endpoint: agregar al controller existente; mantener consistencia de rutas y códigos de estado
- Datos: si se deja InMemory, cambiar el provider de DbContext y agregar migraciones de EF Core
- Pruebas: usar el archivo .http o un cliente REST; verificar códigos 2xx/4xx

## Mantenimiento de documentación
Al evolucionar la solución (cambios manuales, por agentes o PRs), actualizar la documentación:
- README.md: visión general, cómo ejecutar, URLs base, endpoints relevantes y pasos de prueba.
- docs/AGENTS.md: modelo de dominio, endpoints, seed, convenciones y lineamientos de cambio.
- Verificar enlaces internos y rutas. Documentar breaking changes/versionado, cambios de puertos o de provider de datos (y pasos de migración si aplica).

## Restricciones
- Objetivo .NET 9 y C# 13
- Mantener rutas existentes estables; evitar breaking changes sin versionado
- Proteger PII en logs; no exponer datos sensibles en respuestas
