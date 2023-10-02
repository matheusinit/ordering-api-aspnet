namespace OrderingApi.Config;

public static class DatabaseConnection
{
    public static string GetConnectionString()
    {
        if (Env.DB_HOST == null)
            return "Server=192.168.0.18,1433;User Id=sa;Database=master;Trusted_Connection=false;Password=PandaNinja13.;TrustServerCertificate=true;";

        return $"Server={Env.DB_HOST},{Env.DB_PORT};User Id={Env.DB_USER};Database={Env.DB_NAME};Trusted_Connection=false;Password={Env.DB_PASSWORD};TrustServerCertificate=true;";
    }
}
