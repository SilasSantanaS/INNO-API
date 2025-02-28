using INNO.Domain.Models;
using INNO.Domain.Settings;
using INNO.Infra.Interfaces;
using INNO.Infra.Interfaces.Repositories;
using Microsoft.Extensions.Options;

namespace INNO.Infra.Repositories
{
    public class AddressRepository : Repository, IAddressRepository
    {
        public AddressRepository(
            IDbConnectionFactory dbConnectionFactory, 
            CurrentSession session, 
            IOptionsSnapshot<DbSettings> settings
        ) : base(dbConnectionFactory, session, settings)
        {
            tableName = "inno_addresses";
            tableCols =
            [
                "id",
                "city",
                "state",
                "street",
                "number",
                "zip_code",
                "tenant_id",
                "complement",
                "neighborhood",
            ];
        }

        public async Task<Address> CreateAddress(Address data)
        {
            var qry = $@"INSERT INTO {tableName} (
                            city,                            
                            state,
                            street,
                            number,
                            zip_code,
                            tenant_id,
                            complement,
                            neighborhood
                         ) VALUES (
                            @{nameof(data.City)},
                            @{nameof(data.State)},
                            @{nameof(data.Street)},
                            @{nameof(data.Number)},
                            @{nameof(data.ZipCode)},
                            @{nameof(data.TenantId)},
                            @{nameof(data.Complement)},
                            @{nameof(data.Neighborhood)}
                         ) RETURNING 
                            {GetCols()};";

            return await QueryFirstAsync<Address>(qry, data);
        }

        public Task<bool> DeleteAddress(int? id, int? tenantId)
        {
            throw new NotImplementedException();
        }

        public async Task<Address> GetAddressById(int? id, int? tenantId)
        {
            var qry = $@"SELECT 
                            {GetCols()}
                         FROM
                            {tableName}
                         WHERE
                            id = @ID
                            AND tenant_id = @TENANTID;";

            return await QueryFirstAsync<Address>(qry, new
            {
                ID = id,
                TENANTID = tenantId
            });
        }

        public async Task<Address?> UpdateAddress(int? id, Address data)
        {
            if (id == null)
            {
                return null;
            }

            var updatedCols = new List<string>();

            if (!string.IsNullOrWhiteSpace(data.City))
            {
                updatedCols.Add($"city = @{nameof(data.City)}");
            }

            if (!string.IsNullOrWhiteSpace(data.State))
            {
                updatedCols.Add($"state = @{nameof(data.State)}");
            }

            if (!string.IsNullOrWhiteSpace(data.Street))
            {
                updatedCols.Add($"street = @{nameof(data.Street)}");
            }

            if (!string.IsNullOrWhiteSpace(data.Number))
            {
                updatedCols.Add($"number = @{nameof(data.Number)}");
            }

            if (!string.IsNullOrWhiteSpace(data.ZipCode))
            {
                updatedCols.Add($"zip_code = @{nameof(data.ZipCode)}");
            }

            if (!string.IsNullOrWhiteSpace(data.Complement))
            {
                updatedCols.Add($"complement = @{nameof(data.Complement)}");
            }

            if (!string.IsNullOrWhiteSpace(data.Neighborhood))
            {
                updatedCols.Add($"neighborhood = @{nameof(data.Neighborhood)}");
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

            return await QueryFirstAsync<Address>(qry, data);
        }
    }
}
