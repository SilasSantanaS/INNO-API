using INNO.Domain.Models;
using INNO.Domain.Settings;
using INNO.Infra.Interfaces;
using INNO.Infra.Interfaces.Repositories;
using Microsoft.Extensions.Options;

namespace INNO.Infra.Repositories
{
    public class ContactRepository : Repository, IContactRepository
    {
        public ContactRepository(
            IDbConnectionFactory dbConnectionFactory, 
            CurrentSession session, 
            IOptionsSnapshot<DbSettings> settings
        ) : base(dbConnectionFactory, session, settings)
        {
            tableName = "inno_contacts";
            tableCols =
            [
                "id",
                "obs",
                "name",
                "phone",
                "email",
                "tenant_id",
            ];
        }

        public async Task<Contact> CreateContact(Contact data)
        {
            var qry = $@"INSERT INTO {tableName} (
                            obs,
                            name,
                            phone,
                            email,
                            tenant_id
                         ) VALUES (
                            @{nameof(data.Obs)},
                            @{nameof(data.Name)},
                            @{nameof(data.Phone)},
                            @{nameof(data.Email)},
                            @{nameof(data.TenantId)}
                         ) RETURNING *;";

            return await QueryFirstAsync<Contact>(qry, data);
        }

        public Task<bool> DeleteContact(int? id, int? tenantId)
        {
            throw new NotImplementedException();
        }

        public async Task<Contact?> GetContactById(int? id, int? tenantId)
        {
            if (!id.HasValue || !tenantId.HasValue)
            {
                return null;
            }

            var qry = $@"SELECT
                            {GetCols()}
                         FROM
                            {tableName}
                         WHERE
                            id = @ID
                            AND tenant_id = @TENANTID
                         RETURNING *;";

            return await QueryFirstAsync<Contact>(qry, new
            {
                ID = id,
                TENANTID = tenantId,
            });
        }

        public async Task<Contact?> UpdateContact(int? id, Contact data)
        {
            if (id == null)
            {
                return null;
            }

            var updatedCols = new List<string>();

            if (!string.IsNullOrWhiteSpace(data.Obs))
            {
                updatedCols.Add($"obs = @{nameof(data.Obs)}");
            }

            if (!string.IsNullOrWhiteSpace(data.Name))
            {
                updatedCols.Add($"name = @{nameof(data.Name)}");
            }

            if (!string.IsNullOrWhiteSpace(data.Phone))
            {
                updatedCols.Add($"phone = @{nameof(data.Phone)}");
            }

            if (!string.IsNullOrWhiteSpace(data.Email))
            {
                updatedCols.Add($"email = @{nameof(data.Email)}");
            }

            if (!updatedCols.Any())
            {
                return null;
            }

            var qry = $@"UPDATE 
                            {tableName} 
                         SET 
                            {string.Join(",", updatedCols)}
                         WHERE 
                            id = @{nameof(data.Id)}                             
                            AND tenant_id = @{nameof(data.TenantId)} 
                         RETURNING {GetCols()};";

            data.Id = id.Value;

            return await QueryFirstAsync<Contact>(qry, data);
        }
    }
}
