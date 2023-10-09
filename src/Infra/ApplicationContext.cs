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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Order>()
            .HasOne(o => o.Product)
            .WithMany(p => p.Orders)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Product>().HasMany(p => p.Orders).WithOne(o => o.Product);
    }

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options) { }

    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
}
