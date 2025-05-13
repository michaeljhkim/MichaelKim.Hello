// aspire already handles the base projects "using" calls, but any other packages need to explicitly be called
using Npgsql; 

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();

/*
- Connecting to Postgresql server database (Supabase) - configured by connection string in apphost
- Connection String is taken from Azure's environment config
*/
builder.AddNpgsqlDataSource("hellodb");


// Standard boilerplate code
builder.Services.AddEndpointsApiExplorer();
var app = builder.Build();
app.UseHttpsRedirection();
app.MapDefaultEndpoints();


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
        await using var command = new NpgsqlCommand("SELECT " + query_name + " FROM " + table_name + " LIMIT 1", connection);
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


app.Run();