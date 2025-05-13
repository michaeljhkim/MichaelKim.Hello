var builder = DistributedApplication.CreateBuilder(args);

// Connection String is taken from Azure's environment config
var connectionString = builder.AddConnectionString("hellodb");

/*
- Create postgresql server. Added default values for testing.
- Will not use this in production due to costs. Instead, a postgresql server was created and hosted by 'supabase'.
*/
/*
var databaseName = "hellodb";
var postgresdb = builder.AddPostgres("postgres")
    .WithEnvironment("POSTGRES_DB", databaseName)
    .WithBindMount(
        "../michaelkim.hello.backend.ApiService/data/postgres",
        "/docker-entrypoint-initdb.d"
    )
    .AddDatabase(databaseName);
*/

builder.AddProject<Projects.michaelkim_hello_backend_ApiService>("MKapiservice")
    .WithExternalHttpEndpoints()
    .WithReference(connectionString)
    .WaitFor(connectionString);

// npm app will be hosted on a different server, this was for development testing.
/*
builder.AddNpmApp("react", "../../michaelkim.hello.frontend")
    .WithReference(apiService)
    .WaitFor(apiService)
    .WithEnvironment("BROWSER", "none") // Disable opening browser on npm start
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();
*/

builder.Build().Run();
