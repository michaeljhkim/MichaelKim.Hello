using Npgsql; 
public class AgeUpdateService : BackgroundService {
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<AgeUpdateService> _logger;

    public AgeUpdateService(IServiceProvider serviceProvider, ILogger<AgeUpdateService> logger) {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        while (!stoppingToken.IsCancellationRequested) {
            try {
                await RunAgeUpdate(stoppingToken);
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error running daily age update.");
            }

            // Wait until the next day (24 hours)
            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }
    }

    private async Task RunAgeUpdate(CancellationToken cancellationToken) {

        // connect to postgresql database
        using var scope = _serviceProvider.CreateScope();
        var connection = scope.ServiceProvider.GetRequiredService<NpgsqlConnection>();
        await connection.OpenAsync(cancellationToken);

        var cmd = new NpgsqlCommand(@"
            UPDATE hello_info
            SET age = DATE_PART('year', AGE(CURRENT_DATE, birth_date));"
            , connection
        );

        int updated = await cmd.ExecuteNonQueryAsync(cancellationToken);
        _logger.LogInformation("Updated age for {Count} people", updated);
    }
}
