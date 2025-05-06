using Microsoft.EntityFrameworkCore;

namespace AspireExistingDB.ApiService;

public class HelloDbContext(DbContextOptions options) : DbContext(options) {
    public DbSet<HelloInformation> Forecasts => Set<HelloInformation>();
}
