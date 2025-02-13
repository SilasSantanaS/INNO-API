using INNO.Application.Interfaces.Services;
using INNO.Domain.Constants;
using INNO.Domain.Filters;
using INNO.Domain.ViewModels.v1;
using INNO.Domain.ViewModels.v1.Tenants;
using INNO.Domain.ViewModels.v1.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace INNO.Presentation.API.Controllers.v1
{
    [Authorize]
    [ApiController]
    [ApiVersion("1")]
    [Route("tenants")]
    public class TenantsController : ControllerBase
    {
        [HttpPost("")]
        [Authorize(CustomPolicies.ApplicationManager)]
        public async Task<ActionResult<TenantResponseVM>> CreateTenant(
            [FromBody] TenantRequestVM data,
            [FromServices] ITenantService service
        )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await service.CreateTenant(data);

            if (result == null || result?.Message != null)
            {
                return BadRequest(new ResponseVM(400, result?.Message));
            }

            return Ok(result?.Result);
        }

        [HttpPost("{id}/activate")]
        [Authorize(CustomPolicies.ApplicationManager)]
        public async Task<ActionResult<TenantResponseVM>> ActivateTenant(
            int id,
            [FromServices] ITenantService service
        )
        {
            var result = await service.ActivateTenant(id);

            if (result)
            {
                return NoContent();
            }

            return BadRequest();
        }

        [HttpPut("{id}")]
        [Authorize(CustomPolicies.ApplicationManager)]
        public async Task<ActionResult<UserResponseVM>> UpdateTenant(
            int id,
            [FromBody] TenantRequestVM data,
            [FromServices] ITenantService service
        )
        {
            var result = await service.UpdateTenant(id, data);

            if(result == null)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(CustomPolicies.ApplicationManager)]
        public async Task<ActionResult<TenantResponseVM>> InactivateTenant(
            int id,
            [FromServices] ITenantService service
        )
        {
            var result = await service.InactivateTenant(id);

            if (result)
            {
                return NoContent();
            }

            return BadRequest();
        }

        [HttpGet("")]
        [Authorize(CustomPolicies.ApplicationManager)]
        public async Task<ActionResult<TenantListResponseVM>> ListTenants(
            [FromQuery] TenantFilter filter,
            [FromServices] ITenantService service
        )
        {
            return Ok(await service.ListTenants(filter));
        }

        [HttpGet("{id}")]
        [Authorize(CustomPolicies.ApplicationManager)]
        public async Task<ActionResult<TenantResponseVM>> GetTenantById(
            int id,
            [FromServices] ITenantService service
        )
        {
            var result = await service.GetTenantById(id);

            if (result is null)
            {
                return NotFound(new ResponseVM(404, $"Tenant não encontrado."));
            }

            return Ok(result);
        }
    }
}
