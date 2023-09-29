namespace OrderingApi.Config;

public class DatabaseConnection
{
    public static string GetConnectionString()
    {
        return $"Server={Env.DB_HOST},{Env.DB_PORT};User Id={Env.DB_USER};Database={Env.DB_NAME};Trusted_Connection=false;Password={Env.DB_PASSWORD};TrustServerCertificate=true;";
    }
}
