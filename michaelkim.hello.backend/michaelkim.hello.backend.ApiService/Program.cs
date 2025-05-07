// aspire already handles the base projects "using" calls, but any other packages need to explicitly be called
using Npgsql; 

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();
builder.AddNpgsqlDataSource(connectionName: "HelloDB");

// Add CORS service and specify allowed origins - need to directly note the address due to extension policy
builder.Services.AddCors(options => {
    options.AddPolicy("AllowReactApp", policy => {
        policy.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // Optional but can help in dev
    });
});

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Use CORS middleware
app.UseCors("AllowReactApp");
/*
app.UseCors(static builder => 
    builder.AllowAnyMethod()
        .AllowAnyHeader()
        .AllowAnyOrigin());
*/

app.UseHttpsRedirection();
app.MapDefaultEndpoints();

// Hello World endpoint
app.MapGet("/helloworld", () => Results.Ok("Hello World from .NET!"))
   .WithName("GetHelloWorld");


// --- TEST QUERYING ENDPOINTS ---
app.MapGet("/hello_information1", async (NpgsqlDataSource dataSource) => {
    await using var connection = await dataSource.OpenConnectionAsync();
    await using var cmd = new NpgsqlCommand("SELECT id, name FROM items", connection);
    await using var reader = await cmd.ExecuteReaderAsync();

    var items = new List<Dictionary<string, object>>();
    while (await reader.ReadAsync()) {
        var item = new Dictionary<string, object> {
            { "Id", reader.GetInt32(0) },
            { "Name", reader.GetString(1) }
        };
        items.Add(item);
    }

    return Results.Ok(items);
});

// From nuget: https://www.nuget.org/packages/Npgsql.DependencyInjection
app.MapGet("/hello_information2", async (NpgsqlConnection connection) => {
    await connection.OpenAsync();
    await using var command = new NpgsqlCommand("SELECT first_name FROM hello_information LIMIT 1", connection);
    return "Hello World: " + await command.ExecuteScalarAsync();
});



app.Run();