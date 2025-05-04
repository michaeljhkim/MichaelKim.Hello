var builder = DistributedApplication.CreateBuilder(args);

//var connectionString = builder.AddConnectionString("postgresql");

var apiService = builder.AddProject<Projects.michaelkim_hello_backend_ApiService>("MKapiservice")
    //.WithHttpsHealthCheck("/health")
    //.WithReference(connectionString)
    .WithExternalHttpEndpoints();

builder.AddNpmApp("react", "../../michaelkim.hello.frontend")
    .WithReference(apiService)
    .WaitFor(apiService)
    .WithEnvironment("BROWSER", "none") // Disable opening browser on npm start
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.Build().Run();
