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
            optionsBuilder.UseSqlServer(
                $"Server={Env.DB_HOST},{Env.DB_PORT};User Id={Env.DB_USER};Database={Env.DB_NAME};Trusted_Connection=false;Password={Env.DB_PASSWORD};TrustServerCertificate=true;"
            );
        }
    }

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options) { }

    public DbSet<Product> Products { get; set; }
}
