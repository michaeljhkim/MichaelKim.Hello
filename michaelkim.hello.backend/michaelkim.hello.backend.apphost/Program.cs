var builder = DistributedApplication.CreateBuilder(args);

/*
var hello_webapi = builder.AddProject<Projects.michaelkim_hello_backend_webapi>("webapi")
                            .WithReference(postgresdb)
                            .WithEndpoint("http", port: 5000);
*/

var postgres = builder.AddPostgres("postgres");
var postgresdb = postgres.AddDatabase("postgresdb");
var hello_webapi = builder.AddProject<Projects.michaelkim_hello_backend_webapi>("webapi")
                            .WithReference(postgresdb);

builder.Build().Run();
