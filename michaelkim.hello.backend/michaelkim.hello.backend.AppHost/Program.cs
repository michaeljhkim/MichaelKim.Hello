var builder = DistributedApplication.CreateBuilder(args);

// WithBindMount requires absolute file path in relation to host and not dev container
string host_absolute = Environment.GetEnvironmentVariable("HOST_WORKSPACE");    

/*
Create postgresql server. Added default values for testing.
*/
var databaseName = "hellodb";
var postgresdb = builder.AddPostgres("postgres")
    .WithEnvironment("POSTGRES_DB", databaseName)
    .WithBindMount(
        host_absolute + "/michaelkim.hello.backend/michaelkim.hello.backend.ApiService/data/postgres",
        "/docker-entrypoint-initdb.d"
    )
    .AddDatabase(databaseName);

// Add the default database to the application model so that it can be referenced by other resources.
//var postgresdb = postgres.AddDatabase(databaseName);

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


builder.Build().Run();
