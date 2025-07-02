using Microsoft.Extensions.Configuration;
using System.Text;


namespace RepositoryHelper
{
    public static class EncryptionHelper
    {
        public static string GenerateConnectionString(string serverName, string port, string dbName, string user, string password) => $"Server={serverName};Port={port};Database={dbName};user={user};password={password};";

        public static string Fist(string item)
        {
            var final = Convert.ToBase64String(Encoding.UTF8.GetBytes(item));

            return final;
        }

        public static string UnFist(string item) => Encoding.UTF8.GetString(Convert.FromBase64String(item));

        public static string GetAppConnectionString(string connectionName, IConfiguration configuration, string? envName)
        {
            if (envName == "Production")
            {
                var value = Environment.GetEnvironmentVariable(connectionName) ?? throw new Exception("Connection string cannot be null");
                return UnFist(value);

            }
            else
            {
                var value = configuration.GetConnectionString(connectionName)!;
                return value;
                // Local is not encripted
                //   return UnFist(value);

            }

        }
    }
}
