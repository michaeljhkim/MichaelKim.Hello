var builder = DistributedApplication.CreateBuilder(args);

//var connectionString = builder.AddConnectionString("postgresql");
var postgres = builder.AddPostgres("postgres");
                      //.WithPgAdmin();

var databaseName = "HelloDB";
var creationScript = $$"""
    -- Create the database
    CREATE DATABASE {{databaseName}};
""";

/*
    -- Create the table
    CREATE TABLE hello_information (
        person_id INT PRIMARY KEY,
        first_name VARCHAR(50),
        last_name VARCHAR(50),
        age INT,
        email VARCHAR(50),
        github VARCHAR(50),
        birth_date DATE
    );

    INSERT INTO hello_information (person_id, first_name, last_name, age, email, github, birth_date)
    VALUES (0, 'Michael', 'Kim', 21, 'michaelkimwork47@gmail.com', 'https://github.com/michaeljhkim', '2003-09-08');
*/

var db = postgres.AddDatabase(databaseName)
                 .WithCreationScript(creationScript);


var apiService = builder.AddProject<Projects.michaelkim_hello_backend_ApiService>("MKapiservice")
    //.WithHttpsHealthCheck("/health")
    //.WithReference(connectionString)
    .WithExternalHttpEndpoints()
    .WithReference(db)
    .WaitFor(db);

builder.AddNpmApp("react", "../../michaelkim.hello.frontend")
    .WithReference(apiService)
    .WaitFor(apiService)
    .WithEnvironment("BROWSER", "none") // Disable opening browser on npm start
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.Build().Run();
