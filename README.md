# Turnero Médico

Sistema de turnos médicos (.NET 9, ASP.NET Core + React).

- Documentación para agentes y developers: [docs/AGENTS.md](docs/AGENTS.md)

## Proyectos
- TurneroMedico.Server (Web API)
- TurneroMedico.AppHost (Aspire AppHost)
- TurneroMedico.ServiceDefaults (defaults compartidos)
- turneromedico.client (React + Vite)

## Ejecutar (dev)
1) dotnet run -p TurneroMedico.Server
2) API: https://localhost:7120 o http://localhost:5110
3) OpenAPI (dev): /openapi/v1.json

## Endpoints principales
- api/Personas, api/Medicos, api/Pacientes, api/Especialidades, api/ObrasSociales, api/Turnos

## Seed
Se carga al inicio (InMemory): especialidades, obras sociales y 3+ médicos por especialidad.