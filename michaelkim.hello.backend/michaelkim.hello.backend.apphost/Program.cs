var builder = DistributedApplication.CreateBuilder(args);

/*
var hello_webapi = builder.AddProject<Projects.michaelkim_hello_backend_webapi>("webapi")
                            .WithReference(postgresdb)
                            .WithEndpoint("http", port: 5000);
*/


/*
var postgres = builder.AddPostgres("postgres")
                      .WithPgAdmin();
*/
var postgres = builder.AddPostgres("postgres");
//var postgresdb = postgres.AddDatabase("postgresdb");

// create a new database
var databaseName = "michaelkim_hello_db";
var creationScript = $$"""
    -- Create the database
    CREATE DATABASE {{databaseName}};

    """;
var db = postgres.AddDatabase(databaseName)
                 .WithCreationScript(creationScript);

var hello_webapi = builder.AddProject<Projects.michaelkim_hello_backend_webapi>("webapi")
                            .WithReference(db)
                            .WaitFor(db);

builder.Build().Run();
