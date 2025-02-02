using Dapper;
using INNO.Infra.Interfaces;
using Npgsql;
using System.Data;
using System.Text.RegularExpressions;

namespace INNO.Infra
{
    public class PgConnectionFactory : IDbConnectionFactory
    {
        private string _connectionString;

        public PgConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void SetConnectionString(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void SetUser(string user, string password)
        {
            Regex.Replace(_connectionString, $"(User ID=)([^;])*", $"User ID={user}");
            Regex.Replace(_connectionString, $"(Password=)([^;])*", $"Password={password}");
        }

        public IDbConnection GetConnection()
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;

            return new NpgsqlConnection(_connectionString);
        }
    }
}
