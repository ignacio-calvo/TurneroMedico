var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.TurneroMedico_Server>("turneromedico-server");

builder.Build().Run();
