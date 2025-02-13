using INNO.Domain.Filters;
using INNO.Domain.Models;
using INNO.Domain.Settings;
using INNO.Infra.Interfaces;
using INNO.Infra.Interfaces.Repositories;
using Microsoft.Extensions.Options;

namespace INNO.Infra.Repositories
{
    public class UserRepository : Repository, IUserRepository
    {
        public UserRepository(
            IDbConnectionFactory dbConnectionFactory, 
            CurrentSession session, 
            IOptionsSnapshot<DbSettings> settings
        ) : base(dbConnectionFactory, session, settings)
        {
            tableName = "inno_users";
            tableCols = [
                "id",
                "name",
                "email",
                "password",
                "tenant_id",
                "profile_id"
            ];
        }

        public async Task<User> CreateUser(User data)
        {
            var qry = $@"INSERT INTO {tableName} (
                            name,
                            email,
                            document,
                            tenant_id,
                          ""password"",
                            profile_id
                         ) VALUES (
                            @{nameof(data.Name)}, 
                            @{nameof(data.Email)},
                            @{nameof(data.Document)},
                            @{nameof(data.TenantId)},
                            @{nameof(data.Password)},
                            @{nameof(data.ProfileId)}
                         ) RETURNING {GetCols()};";

            return await QueryFirstAsync<User>(qry, data);
        }

        public async Task<bool> DeleteUser(int id, int tenantId)
        {
            var qry = $@"DELETE FROM 
                            {tableName} 
                         WHERE 
                            id = @ID 
                            AND tenant_id = @TENANTID;";

            return await ExecuteAsync(qry, new
            {
                ID = id,
                TENANTID = tenantId,
            }) > 0;
        }

        public async Task<int> GetTotalItems(UserFilter filter)
        {
            var where = BuildFilter(filter);
            where = string.IsNullOrWhiteSpace(where) ? null : $"WHERE {where}";

            var qry = $@"SELECT
                            count(*)
                         FROM
                            {tableName}
                         {where};";

            return await QueryFirstAsync<int?>(qry, filter) ?? 0;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var qry = $@"SELECT 
                            {GetCols("u")}
                         FROM 
                            {tableName} AS u
                         LEFT JOIN 
                            inno_tenants AS t ON u.tenant_id = t.id
                         WHERE 
                            u.email = @EMAIL
                         LIMIT 1;";

            return await QueryFirstAsync<User>(qry, new
            {
                EMAIL = email
            });
        }

        public async Task<User> GetUserById(int? id)
        {
            var qry = $@"SELECT 
                            {GetCols("u")}
                         FROM 
                            {tableName} AS u
                         WHERE 
                            u.id = @ID
                         LIMIT 1;";

            return await QueryFirstAsync<User>(qry, new
            {
                ID = id,
            });
        }

        public async Task<IEnumerable<User>> ListUsers(UserFilter filter)
        {
            var offset = Paginate(filter.GetPage(), filter.GetPageLimit());

            var where = BuildFilter(filter);
            where = string.IsNullOrWhiteSpace(where) ? null : $"WHERE {where}";

            var qry = $@"SELECT
                            {GetCols()}
                         FROM
                            {tableName}
                         {where}
                         LIMIT 
                            {filter.GetPageLimit()} OFFSET {offset};";

            return await QueryAsync<User>(qry, filter);
        }

        public async Task<User> UpdateUser(int? id, User data)
        {
            if (id == null)
            {
                return null;
            }

            var updatedCols = new List<string>();

            if (!string.IsNullOrWhiteSpace(data.Name))
            {
                updatedCols.Add($"name = @{nameof(data.Name)}");
            }

            if (!string.IsNullOrWhiteSpace(data.Document))
            {
                updatedCols.Add($"document = @{nameof(data.Document)}");
            }

            if (data.ProfileId != 0)
            {
                updatedCols.Add($"profile_id = @{nameof(data.ProfileId)}");
            }

            updatedCols.Add($"updated_at = current_timestamp");

            var qry = $@"UPDATE 
                            {tableName} 
                         SET 
                            {string.Join(",", updatedCols)}
                         WHERE 
                            id = @{nameof(data.Id)}                             
                            AND tenant_id = @{nameof(data.TenantId)} 
                         RETURNING {GetCols()};";

            data.Id = id.Value;

            return await QueryFirstAsync<User>(qry, data);
        }

        public async Task<bool> UpdateUserPassword(int? id, string password)
        {
            var qry = $@"UPDATE 
                            {tableName} 
                         SET 
                            ""password"" = @PASSWORD
                         WHERE 
                            id = @ID;";

            return await ExecuteAsync(qry, new
            {
                ID = id,
                PASSWORD = password
            }) > 0;
        }

        private string BuildFilter(UserFilter filter)
        {
            var result = new List<string>();

            if (filter.TenantId.HasValue)
            {
                result.Add($@"tenant_id = @{nameof(filter.TenantId)}");
            }

            return string.Join(" AND ", result);
        }
    }
}
