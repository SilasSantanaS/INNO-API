using System.Data;

namespace INNO.Infra.Interfaces
{
    public interface IDbConnectionFactory
    {
        IDbConnection GetConnection();
        void SetConnectionString(string connectionString);
        void SetUser(string user, string password);
    }
}
