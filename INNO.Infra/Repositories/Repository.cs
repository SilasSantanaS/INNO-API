using Dapper;
using INNO.Domain.Filters;
using INNO.Domain.Models;
using INNO.Domain.Settings;
using INNO.Infra.Interfaces;
using Microsoft.Extensions.Options;
using Serilog;
using System.Data;

namespace INNO.Infra.Repositories
{
    public class Repository
    {
        protected readonly DbSettings _dbSettings;
        protected readonly CurrentSession _session;
        protected readonly IDbConnectionFactory _connectionFactory;

        protected string tableName;
        protected IEnumerable<string> tableCols { private get; set; }

        protected int MaxAttempts = 0;
        protected int AttemptDelay = 2000;

        protected delegate T QueryMethod<T>(string qry, object data);

        public Repository(
            IDbConnectionFactory dbConnectionFactory,
            CurrentSession session,
            IOptionsSnapshot<DbSettings> settings
        )
        {
            _session = session;
            _dbSettings = settings.Value;
            _connectionFactory = dbConnectionFactory;
        }

        private void SetCurrentTenant()
        {
        }

        protected virtual string GetCols(string prefix = null, IEnumerable<string> except = null)
        {
            var cols = new List<string>();

            foreach (var col in tableCols)
            {
                if (!except?.Contains(col) ?? true)
                {
                    if (!string.IsNullOrWhiteSpace(prefix))
                    {
                        cols.Add($"{prefix}.{col}");
                    }
                    else
                    {
                        cols.Add(col);
                    }
                }
            }

            return string.Join(",", cols);
        }

        public int Paginate(ListFilter filter)
        {
            return Paginate(filter.GetPage(), filter.GetPageLimit());
        }

        public int Paginate(int page, int limit = 30)
        {
            if (page < 1)
            {
                page = 1;
            }

            if (limit > 50)
            {
                limit = 50;
            }
            else if (limit < 1)
            {
                limit = 30;
            }

            return ((page - 1) * limit);
        }

        protected virtual async Task<int> ExecuteAsync(string qry, object data, int attempts = 0, IDbTransaction transaction = null)
        {
            try
            {
                using (IDbConnection connection = _connectionFactory.GetConnection())
                {
                    connection.Open();
                    //SetCurrentTenant();

                    return await connection.ExecuteAsync(qry, data, transaction: transaction);
                }
            }
            catch (Exception e)
            {
                if (attempts < MaxAttempts)
                {
                    attempts++;
                    await Task.Delay(AttemptDelay * attempts);

                    return await ExecuteAsync(qry, data, attempts);
                }
                else
                {
                    Log.Error(e.Message);
                    Console.WriteLine(e.Message);
                    throw;
                }
            }
        }

        protected virtual async Task<bool> ExecuteAsync(string qry, int attempts = 0, IDbTransaction transaction = null)
        {
            try
            {
                using (IDbConnection connection = _connectionFactory.GetConnection())
                {
                    connection.Open();
                    //SetCurrentTenant();

                    await connection.ExecuteAsync(qry, transaction: transaction);

                    return true;
                }
            }
            catch (Exception e)
            {
                if (attempts < MaxAttempts)
                {
                    attempts++;
                    await Task.Delay(AttemptDelay * attempts);

                    return await ExecuteAsync(qry, attempts);
                }
                else
                {
                    Log.Error(e.Message);
                    Console.WriteLine(e.Message);
                    throw;
                }
            }
        }

        protected virtual async Task<IEnumerable<T>> QueryAsync<T>(string qry, int attempts = 0, IDbTransaction transaction = null)
        {
            try
            {
                using (IDbConnection connection = _connectionFactory.GetConnection())
                {
                    connection.Open();
                    //SetCurrentTenant();

                    //qry = $@"SET app.tenant_id = {_session.TenantId}; {qry}";

                    var result = await connection.QueryAsync<T>(qry, transaction: transaction);

                    return result;
                }
            }
            catch (Exception e)
            {
                if (attempts < MaxAttempts)
                {
                    attempts++;
                    await Task.Delay(AttemptDelay * attempts);

                    return await QueryAsync<T>(qry, attempts);
                }
                else
                {
                    Log.Error(e.Message);
                    Console.WriteLine(e.Message);
                    throw;
                }
            }
        }

        protected virtual async Task<IEnumerable<T>> QueryAsync<T>(string qry, object data, int attempts = 0, IDbTransaction transaction = null)
        {
            try
            {
                using (IDbConnection connection = _connectionFactory.GetConnection())
                {
                    connection.Open();
                    //SetCurrentTenant();

                    //qry = $@"SET app.tenant_id = {_session.TenantId}; {qry}";

                    var result = await connection.QueryAsync<T>(qry, data, transaction: transaction);

                    return result;
                }
            }
            catch (Exception e)
            {
                if (attempts < MaxAttempts)
                {
                    attempts++;
                    await Task.Delay(AttemptDelay * attempts);

                    return await QueryAsync<T>(qry, data, attempts);
                }
                else
                {
                    Log.Error(e.Message);
                    Console.WriteLine(e.Message);
                    throw;
                }
            }
        }

        protected virtual async Task<T> QueryFirstAsync<T>(string qry, object data, int attempts = 0, IDbTransaction transaction = null)
        {
            try
            {
                using (IDbConnection connection = _connectionFactory.GetConnection())
                {
                    connection.Open();
                    //SetCurrentTenant();

                    var result = await connection.QueryFirstOrDefaultAsync<T>(qry, data, transaction: transaction);

                    return result;
                }

            }
            catch (Exception e)
            {
                if (attempts < MaxAttempts)
                {
                    attempts++;
                    await Task.Delay(AttemptDelay * attempts);

                    return await QueryFirstAsync<T>(qry, data, attempts);
                }
                else
                {
                    Log.Error(e.Message);
                    Console.WriteLine(e.Message);
                    throw;
                }
            }
        }

        protected virtual async Task<T> QueryFirstAsync<T>(string qry, int attempts = 0, IDbTransaction transaction = null)
        {
            try
            {
                using (IDbConnection connection = _connectionFactory.GetConnection())
                {
                    connection.Open();
                    //SetCurrentTenant();

                    var result = await connection.QueryFirstOrDefaultAsync<T>(qry, transaction: transaction);

                    return result;
                }
            }
            catch (Exception e)
            {
                if (attempts < MaxAttempts)
                {
                    attempts++;
                    await Task.Delay(AttemptDelay * attempts);

                    return await QueryFirstAsync<T>(qry, attempts);
                }
                else
                {
                    Log.Error(e.Message);
                    Console.WriteLine(e.Message);
                    throw;
                }
            }
        }

        protected virtual T QueryFirst<T>(string qry, object data, int attempts = 0, IDbTransaction transaction = null)
        {
            try
            {
                using (IDbConnection connection = _connectionFactory.GetConnection())
                {
                    connection.Open();
                    //SetCurrentTenant();

                    var result = connection.QueryFirstOrDefault<T>(qry, data, transaction: transaction);

                    return result;
                }
            }
            catch (Exception e)
            {
                if (attempts < MaxAttempts)
                {
                    attempts++;
                    return QueryFirst<T>(qry, data, attempts);
                }
                else
                {
                    Log.Error(e.Message);
                    Console.WriteLine(e.Message);
                    throw;
                }
            }
        }

        protected virtual bool Execute(string qry, object data, int attempts = 0)
        {
            try
            {
                using (IDbConnection connection = _connectionFactory.GetConnection())
                {
                    connection.Open();
                    //SetCurrentTenant();

                    connection.Execute(qry, data);

                    return true;
                }
            }
            catch (Exception e)
            {
                if (attempts < MaxAttempts)
                {
                    attempts++;
                    return Execute(qry, data, attempts);
                }
                else
                {
                    Log.Error(e.Message);
                    Console.WriteLine(e.Message);
                    throw;
                }
            }
        }

        protected virtual bool Execute(string qry, int attempts = 0)
        {
            try
            {
                using (IDbConnection connection = _connectionFactory.GetConnection())
                {
                    connection.Open();
                    //SetCurrentTenant();

                    connection.Execute(qry);

                    return true;
                }
            }
            catch (Exception e)
            {
                if (attempts < MaxAttempts)
                {
                    attempts++;
                    return Execute(qry, attempts);
                }
                else
                {
                    Log.Error(e.Message);
                    Console.WriteLine(e.Message);
                    throw;
                }
            }
        }
    }
}
