// aspire already handles the base projects "using" calls, but any other packages need to explicitly be called
using Npgsql;
using Microsoft.Extensions.Caching.Memory; 

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();

/*
- Connecting to Postgresql server database (Supabase) - configured by connection string in apphost
- Connection String is taken from Azure's environment config
*/
builder.AddNpgsqlDataSource("hellodb");

// Register all controllers
builder.Services.AddControllers();

// Register Age Update Hosted service
builder.Services.AddHostedService<AgeUpdateService>();

// Register Pinned Repo Hosted Service 
builder.Services.AddMemoryCache();
builder.Services.AddHttpClient();
builder.Services.AddHostedService<PinnedRepoUpdateService>();

// Standard boilerplate code
builder.Services.AddEndpointsApiExplorer();
var app = builder.Build();
app.UseHttpsRedirection();
app.MapDefaultEndpoints();
app.MapControllers();


/*
--- SQL QUERYING --- (From nuget: https://www.nuget.org/packages/Npgsql.DependencyInjection)
*/

/*
- This is a function that does a basic query to the postgresql server, then exposes the resulting value as an endpoint.
- At the time of writing, the hello_info table has one entry with all of my information.  
- This function will be modified or a new function will be created if advanced querying is required.

- 'query_name' is the name of the column you want to query from in the 'hello_info' table.
- 'endpoint_name' is the name of the endpoint that the frontend will call.
- 'table_name' is the name of the table in the database being queried. Mostly for reminder.
*/
void simple_query_endpoint(string query_name, string? endpoint_name=null, string table_name="hello_info") {
    endpoint_name = "/" + (endpoint_name ?? query_name);    // if the user defines endpoint_name it will be used, else use query_name 

    app.MapGet(endpoint_name, async (NpgsqlConnection connection) => {
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand("SELECT " + query_name + " FROM " + table_name + " WHERE id = 4", connection);
        return await command.ExecuteScalarAsync();
    });
}

simple_query_endpoint("first_name");
simple_query_endpoint("last_name");
simple_query_endpoint("age");
simple_query_endpoint("email");
simple_query_endpoint("birth_date");

// Specific endpoint_name is needed here, because the react typescript also displays endpoint_name. Done to reduce variables in frontend.
simple_query_endpoint("github", "Github");
simple_query_endpoint("linkedin", "LinkedIn");


app.MapGet("/pinned-repos", (IMemoryCache cache) => {
    if (cache.TryGetValue<List<PinnedRepo>>("PinnedRepos", out var repos)) {
        return Results.Ok(repos);
    }
    return Results.Problem("Pinned repositories not loaded yet. Please try again later.");
});



//cmd.CommandText = "SELECT COUNT(*) FROM table_name";
/*
void all_endpoints(string table_name="github_repos") {
    // get the connection string from builder and then use it to query the number of rows in table
    var connectionString = builder.Configuration.GetConnectionString("hellodb");
    using var connection = new NpgsqlConnection(connectionString);
    connection.Open();
    using var command = new NpgsqlCommand("SELECT COUNT(*) FROM " + table_name, connection);
    var count = (long)command.ExecuteScalar();      // ExecuteScalar returns the first column of the first row (here: the count)

    for (int i = 1; i <= count; i++) { 
        app.MapGet(endpoint_name, async (NpgsqlConnection connection) => {
            await connection.OpenAsync();
            await using var command = new NpgsqlCommand("SELECT " + query_name + " FROM " + table_name + " WHERE id = " + i, connection);
            return await command.ExecuteScalarAsync();
        });
    }
}
*/

/*
SELECT * FROM (SELECT *, ROW_NUMBER() OVER (ORDER BY name) AS row_num FROM github_repos) sub WHERE row_num = 5;

"SELECT " + query_name + " FROM (SELECT *, ROW_NUMBER() OVER (ORDER BY " name ") AS row_num FROM " table_name ") sub WHERE row_num = " + 5
*/


app.Run();