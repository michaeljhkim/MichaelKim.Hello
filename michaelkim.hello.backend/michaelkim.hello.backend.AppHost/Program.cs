var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres");
var postgresdb = postgres.AddDatabase("hellodb");

var apiService = builder.AddProject<Projects.michaelkim_hello_backend_ApiService>("MKapiservice")
    //.WithHttpsHealthCheck("/health")
    //.WithReference(connectionString)
    .WithExternalHttpEndpoints()
    .WithReference(postgresdb)
    .WaitFor(postgresdb);

builder.AddNpmApp("react", "../../michaelkim.hello.frontend")
    .WithReference(apiService)
    .WaitFor(apiService)
    .WithEnvironment("BROWSER", "none") // Disable opening browser on npm start
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();


await builder.Build().RunAsync();
