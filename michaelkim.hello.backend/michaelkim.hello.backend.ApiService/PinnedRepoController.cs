using Microsoft.AspNetCore.Mvc;
using Npgsql;

/*
VERY UNFINISHED AT THE MOMENT, TOO EXHAUSTED RN

THIS CONTROLLER WILL BE USED TO BE ABLE TO SEND A LIST OF STRINGS OF EACH COLUMN VALUE IN A SPECIFIED ROW
*/

[ApiController]
[Route("[controller]")]
public class PinnedRepoController : ControllerBase {
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<PinnedRepoController> _logger;
    private static readonly HashSet<string> AllowedTables = new(StringComparer.OrdinalIgnoreCase) { "github_repos" };
    private static readonly HashSet<string> AllowedOrderBy = new(StringComparer.OrdinalIgnoreCase) { "name", "description", "link" };

    public PinnedRepoController(IServiceProvider serviceProvider, ILogger<PinnedRepoController> logger) {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

   [HttpGet("{table_name}/{order_by}")]
    public async Task<IActionResult> Get(string table_name = "github_repos", string order_by = "name") {
        _logger.LogInformation("Received request for pinned repos: table = {Table}, orderBy = {OrderBy}", table_name, order_by);

        // check if the request is valid - prevents malicious sql injection
        if (!AllowedTables.Contains(table_name) || !AllowedOrderBy.Contains(order_by))
        {
            _logger.LogWarning("Invalid request with table = {Table}, orderBy = {OrderBy}", table_name, order_by);
            return BadRequest("Invalid table name or order by column.");
        }

        try {
            // connect to postgresql database
            using var scope = _serviceProvider.CreateScope();
            var connection = scope.ServiceProvider.GetRequiredService<NpgsqlConnection>();
            await connection.OpenAsync();

            _logger.LogInformation("Successfully connected to database.");

            // list of each entry in the table
            var pinned_repos = new List<PinnedRepo>();  
            var query = $"""
                SELECT name, description, link, uuid
                FROM (
                    SELECT *, ROW_NUMBER() 
                    OVER (ORDER BY {order_by}) AS row_num
                    FROM {table_name}
                ) sub
                """;

            _logger.LogDebug("Executing SQL query: {Query}", query);

            await using var command = new NpgsqlCommand(query, connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync()) {
                pinned_repos.Add(new PinnedRepo {
                    Name        = reader["name"]        as string ?? string.Empty,
                    Description = reader["description"] as string ?? string.Empty,
                    Link        = reader["link"]        as string ?? string.Empty,
                    uuid        = reader["uuid"]        as string ?? string.Empty
                });
            }

            _logger.LogInformation("Fetched {Count} pinned repos from {Table}.", pinned_repos.Count, table_name);
            return Ok(pinned_repos);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error occurred while fetching pinned repos.");
            return StatusCode(500, "Internal server error.");
        }
    }
}
