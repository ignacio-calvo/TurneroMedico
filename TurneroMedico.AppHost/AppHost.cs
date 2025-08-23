var builder = DistributedApplication.CreateBuilder(args);

var server = builder.AddProject<Projects.TurneroMedico_Server>("turneromedico-server");

// React client (Vite) via Node/NPM app
var client = builder.AddNpmApp("turneromedico-client", "../turneromedico.client", scriptName: "dev")
    .WithEnvironment("BROWSER", "none")
    .WithEnvironment("VITE_API_BASE_URL", server.GetEndpoint("https"))
    .WithEnvironment("DEV_SERVER_PORT", "5173") // alinear puerto vite si PORT no se propaga
    .WithEnvironment("VITE_USE_HTTPS", "false")
    .WithReference(server) // asegurar orden de arranque y dependencia
    .WaitFor(server)
    .WithHttpEndpoint(name: "http", port: 5173, env: "PORT")
    .WithExternalHttpEndpoints();

builder.Build().Run();
