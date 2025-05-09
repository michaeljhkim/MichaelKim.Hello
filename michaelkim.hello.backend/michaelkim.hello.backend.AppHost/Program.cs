var builder = DistributedApplication.CreateBuilder(args);

/*
Create postgresql server. Added default values for testing.
*/
var databaseName = "hellodb";
var postgresdb = builder.AddPostgres("postgres")
    .WithEnvironment("POSTGRES_DB", databaseName)
    .WithBindMount(
        "../michaelkim.hello.backend.ApiService/data/postgres",
        "/docker-entrypoint-initdb.d"
    )
    .AddDatabase(databaseName);

var apiService = builder.AddProject<Projects.michaelkim_hello_backend_ApiService>("MKapiservice")
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


builder.Build().Run();
