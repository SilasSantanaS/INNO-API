using INNO.Domain.Settings;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Data;

namespace INNO.Infra
{
    public sealed class DbSession : IDisposable
    {
        public IDbConnection Connection { get; }
        public IDbTransaction Transaction { get; set; }

        public DbSession(IOptionsSnapshot<DbSettings> settings)
        {
            Connection = new NpgsqlConnection(settings.Value.PgConnection);
            Connection.Open();
        }

        public void Dispose() => Connection?.Dispose();
    }
}
