namespace OrderingApi.Config;

using System;
using System.IO;

public static class Env
{
    public static readonly string? PORT = DotEnv.Get("PORT");
    public static readonly string? DB_HOST = DotEnv.Get("DB_HOST");
    public static readonly string? DB_PORT = DotEnv.Get("DB_PORT");
    public static readonly string? DB_NAME = DotEnv.Get("DB_NAME");
    public static readonly string? DB_USER = DotEnv.Get("DB_USER");
    public static readonly string? DB_PASSWORD = DotEnv.Get("DB_PASSWORD");
}

public static class DotEnv
{
    public static void Load(string filePath)
    {
        if (!File.Exists(filePath))
            return;

        foreach (var line in File.ReadAllLines(filePath))
        {
            var parts = line.Split("=", StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 2)
                continue;

            Environment.SetEnvironmentVariable(parts[0], parts[1]);
        }
    }

    public static string? Get(string key)
    {
        if (key == "")
            return null;

        return System.Environment.GetEnvironmentVariable(key);
    }

    public static void Configure()
    {
        var rootPath = Directory.GetParent(".")?.Parent?.Parent?.FullName;

        if (rootPath == null)
            throw new InvalidOperationException("Could not find path for .env file.");

        var dotenvFilePath = Path.Combine(rootPath, ".env");
        Load(dotenvFilePath);
    }
}
