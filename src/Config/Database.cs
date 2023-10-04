namespace OrderingApi.Config;

public static class DatabaseConnection
{
    public static string GetConnectionString()
    {
        var environmentEnvVar = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        var isDevelopment = string.Equals(
            environmentEnvVar,
            "development",
            StringComparison.InvariantCultureIgnoreCase
        );
        var dbHost = Environment.GetEnvironmentVariable("DB_HOST");

        if (isDevelopment)
            return $"Server={dbHost},1433;User Id=sa;Database=master;Trusted_Connection=false;Password=PandaNinja13.;TrustServerCertificate=true;";

        return $"Server={Env.DB_HOST},{Env.DB_PORT};User Id={Env.DB_USER};Database={Env.DB_NAME};Trusted_Connection=false;Password={Env.DB_PASSWORD};TrustServerCertificate=true;";
    }
}
