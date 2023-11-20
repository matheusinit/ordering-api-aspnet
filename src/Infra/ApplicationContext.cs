namespace OrderingApi.Data;

using Microsoft.EntityFrameworkCore;
using OrderingApi.Domain;
using OrderingApi.Config;

public class ApplicationContext : DbContext
{
    public ApplicationContext() { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(DatabaseConnection.GetConnectionString());
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) { }

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options) { }

    public DbSet<Order> Orders { get; set; }
}
