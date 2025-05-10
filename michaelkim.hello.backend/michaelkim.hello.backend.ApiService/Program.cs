// aspire already handles the base projects "using" calls, but any other packages need to explicitly be called
using Npgsql; 

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();
builder.AddNpgsqlDataSource(connectionName: "hellodb");

// Add CORS service and specify allowed origins - need to directly note the address due to extension policy
builder.Services.AddCors(options => {
    options.AddPolicy("AllowReactApp", policy => {
        policy.WithOrigins("http://localhost:3001")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .SetIsOriginAllowed((host) => true);
    });
});

builder.Services.AddEndpointsApiExplorer();
var app = builder.Build();

app.UseHttpsRedirection();
app.MapDefaultEndpoints();

// Use CORS middleware
app.UseCors("AllowReactApp");
/*
app.UseCors(static builder => 
    builder.AllowAnyMethod()
        .AllowAnyHeader()
        .AllowAnyOrigin());
*/


// Hello World endpoint test: http://localhost:5431/helloworld
app.MapGet("/helloworld", () => Results.Ok("Hello World from .NET!"))
   .WithName("GetHelloWorld");

/*
--- SQL QUERYING --- (From nuget: https://www.nuget.org/packages/Npgsql.DependencyInjection)
*/

//NOTE TO SELF: since these are currently just simple queries, I can just make a function that takes in a list of values and loop through these motions

// first name query
app.MapGet("/first_name", async (NpgsqlConnection connection) => {
    await connection.OpenAsync();
    await using var command = new NpgsqlCommand("SELECT first_name FROM hello_info LIMIT 1", connection);
    return await command.ExecuteScalarAsync();
});

// last name query
app.MapGet("/last_name", async (NpgsqlConnection connection) => {
    await connection.OpenAsync();
    await using var command = new NpgsqlCommand("SELECT last_name FROM hello_info LIMIT 1", connection);
    return await command.ExecuteScalarAsync();
});

// email query
app.MapGet("/email", async (NpgsqlConnection connection) => {
    await connection.OpenAsync();
    await using var command = new NpgsqlCommand("SELECT email FROM hello_info LIMIT 1", connection);
    return await command.ExecuteScalarAsync();
});

// github query
app.MapGet("/Github", async (NpgsqlConnection connection) => {
    await connection.OpenAsync();
    await using var command = new NpgsqlCommand("SELECT github FROM hello_info LIMIT 1", connection);
    return await command.ExecuteScalarAsync();
});

// linkedin query
app.MapGet("/LinkedIn", async (NpgsqlConnection connection) => {
    await connection.OpenAsync();
    await using var command = new NpgsqlCommand("SELECT linkedin FROM hello_info LIMIT 1", connection);
    return await command.ExecuteScalarAsync();
});


app.Run();