using Npgsql;
using HtmlAgilityPack;
using Microsoft.Extensions.Caching.Memory;

/*
- Background service executed once every 24 hours.
- Scrapes data from personal GitHub profile (not available via GitHub API).
- Processes pinned repositories.
- Automates updates to the projects section in Frontend.
*/

public class PinnedRepoUpdateService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<PinnedRepoUpdateService> _logger;
    private readonly IMemoryCache _cache;
    private readonly HttpClient _httpClient;

    public PinnedRepoUpdateService(IServiceProvider serviceProvider, ILogger<PinnedRepoUpdateService> logger, IMemoryCache cache, IHttpClientFactory httpClientFactory)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _cache = cache;
        _httpClient = httpClientFactory.CreateClient();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var repos = await FetchPinnedReposAsync(stoppingToken);
                _cache.Set("PinnedRepos", repos, TimeSpan.FromHours(24));
                _logger.LogInformation("Fetched pinned repositories at {Time}", DateTimeOffset.Now);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch pinned repos");
            }

            await Task.Delay(TimeSpan.FromHours(24), stoppingToken); // wait before next fetch
        }
    }

    private async Task<List<PinnedRepo>> FetchPinnedReposAsync(CancellationToken cancellationToken)
    {
        string githubUsername = "michaeljhkim";
        string baseUrl = "https://github.com";
        string profileUrl = $"{baseUrl}/{githubUsername}";

        // connect to postgresql database
        using var scope = _serviceProvider.CreateScope();
        var connection = scope.ServiceProvider.GetRequiredService<NpgsqlConnection>();
        await connection.OpenAsync(cancellationToken);

        // load html scrapper library
        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("MyAspireGitHubRepoFetcher/1.0");
        var html = await _httpClient.GetStringAsync(profileUrl);

        // load html page
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        // scrap data
        var pinnedRepoCards = doc.DocumentNode.SelectNodes("//div[contains(@class, 'pinned-item-list-item')]");
        var repos = new List<PinnedRepo>();
        using var transaction = await connection.BeginTransactionAsync(cancellationToken);

        if (pinnedRepoCards != null)
        {
            foreach (var card in pinnedRepoCards)
            {
                var linkNode = card.SelectSingleNode(".//a[contains(@class, 'text-bold')]");
                var name = linkNode?.InnerText.Trim();
                var href = linkNode?.GetAttributeValue("href", "").Trim();

                var descNode = card.SelectSingleNode(".//p[contains(@class, 'pinned-item-desc')]");
                var description = descNode?.InnerText.Trim() ?? "";

                if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(href))
                {
                    var fullLink = $"{baseUrl}{href}";
                    repos.Add(new PinnedRepo
                    {
                        Name = name,
                        Description = description,
                        Link = fullLink
                    });

                    using var cmd = new NpgsqlCommand(@"
                        INSERT INTO github_repos (name, description, link)
                        VALUES (@name, @description, @link)
                        ON CONFLICT (name)
                        DO UPDATE SET
                            description = EXCLUDED.description,
                            link = EXCLUDED.link;
                        ",
                        connection
                    );

                    cmd.Parameters.AddWithValue("name", name);
                    cmd.Parameters.AddWithValue("description", description);
                    cmd.Parameters.AddWithValue("link", fullLink);

                    await cmd.ExecuteNonQueryAsync(cancellationToken);
                }
            }
        }
        await transaction.CommitAsync(cancellationToken);
        _logger.LogInformation("Inserted/Updated {Count} pinned repos at {Time}", repos.Count, DateTimeOffset.Now);

        return repos;
    }


}
