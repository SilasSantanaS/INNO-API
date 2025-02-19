using INNO.Application.Interfaces.Services;
using INNO.Domain.Constants;
using INNO.Domain.Filters;
using INNO.Domain.ViewModels.v1;
using INNO.Domain.ViewModels.v1.HealthPlans;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace INNO.Presentation.API.Controllers.v1
{
    [Authorize]
    [ApiController]
    [ApiVersion("1")]
    [Route("health-plans")]
    public class HealthPlansController : ControllerBase
    {
        [HttpGet("")]
        [Authorize(CustomPolicies.ApplicationAdmin)]
        public async Task<ActionResult<HealthPlanResponseVM>> ListHealthPlans(
            [FromQuery] HealthPlanFilter filter,
            [FromServices] IHealthPlanService service
        )
        {
            return Ok(await service.ListHealthPlans(filter));
        }

        [HttpGet("{id}")]
        [Authorize(CustomPolicies.ApplicationAdmin)]
        public async Task<ActionResult<HealthPlanResponseVM>> GetHealthPlanById(
            int id,
            [FromServices] IHealthPlanService service
        )
        {
            var result = await service.GetHealthPlanById(id);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost("")]
        [Authorize(CustomPolicies.ApplicationAdmin)]
        public async Task<ActionResult<HealthPlanResponseVM>> CreateHealthPlan(
            [FromBody] HealthPlanRequestVM data,
            [FromServices] IHealthPlanService service
        )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (result, validation) = await service.CreateHealthPlan(data);

            if (result is null)
            {
                return BadRequest(new ResponseVM(400, validation.Messages));
            }

            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(CustomPolicies.ApplicationAdmin)]
        public async Task<ActionResult<HealthPlanResponseVM>> UpdateHealthPlan(
            int id,
            [FromBody] HealthPlanRequestVM data,
            [FromServices] IHealthPlanService service
        )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await service.UpdateHealthPlan(id, data);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(CustomPolicies.ApplicationAdmin)]
        public async Task<ActionResult<HealthPlanResponseVM>> DeleteHealthPlan(
            int id,
            [FromQuery] int tenantId,
            [FromServices] IHealthPlanService service
        )
        {
            var result = await service.DeleteHealthPlan(id, tenantId);

            if (result)
            {
                return NoContent();
            }

            return BadRequest();
        }
    }
}
