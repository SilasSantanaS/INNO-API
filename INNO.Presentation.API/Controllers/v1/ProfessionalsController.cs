using INNO.Application.Interfaces.Services;
using INNO.Domain.Constants;
using INNO.Domain.ViewModels.v1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using INNO.Domain.ViewModels.v1.Professionals;
using INNO.Domain.Filters;

namespace INNO.Presentation.API.Controllers.v1
{
    [Authorize]
    [ApiController]
    [ApiVersion("1")]
    [Route("professionals")]
    public class ProfessionalsController : ControllerBase
    {
        [HttpPost("")]
        [Authorize(CustomPolicies.ApplicationAdmin)]
        public async Task<ActionResult<ProfessionalResponseVM>> CreateProfessional(
            [FromBody] ProfessionalRequestVM data,
            [FromServices] IProfessionalService service
        )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (result, validation) = await service.CreateProfessional(data);

            if (result is null)
            {
                return BadRequest(new ResponseVM(400, validation.Messages));
            }

            return Ok(result);
        }

        [HttpGet("")]
        [Authorize(CustomPolicies.ApplicationAdmin)]
        public async Task<ActionResult<ProfessionalResponseVM>> ListProfessionals(
            [FromQuery] ProfessionalFilter filter,
            [FromServices] IProfessionalService service
        )
        {
            return Ok(await service.ListProfessionals(filter));
        }

        [HttpGet("{id}")]
        [Authorize(CustomPolicies.ApplicationAdmin)]
        public async Task<ActionResult<ProfessionalResponseVM>> ProfessionalById(
            int id,
            [FromServices] IProfessionalService service
        )
        {
            var result = await service.GetProfessionalById(id);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(CustomPolicies.ApplicationAdmin)]
        public async Task<ActionResult<ProfessionalResponseVM>> UpdateProfessional(
            int id,
            [FromBody] ProfessionalRequestVM data,
            [FromServices] IProfessionalService service
        )
        {
            var result = await service.UpdateProfessional(id, data);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost("{id}/activate")]
        [Authorize(CustomPolicies.ApplicationAdmin)]
        public async Task<ActionResult> ActivateProfessional(
            int id,
            [FromServices] IProfessionalService service
        )
        {
            var (result, _) = await service.ActivateProfessional(id);

            if (result)
            {
                return NoContent();
            }

            return BadRequest();
        }

        [HttpDelete("{id}")]
        [Authorize(CustomPolicies.ApplicationAdmin)]
        public async Task<ActionResult> InactivateProfessional(
            int id,
            [FromServices] IProfessionalService service
        )
        {
            var (result, _) = await service.InactivateProfessional(id);

            if (result)
            {
                return NoContent();
            }

            return BadRequest();
        }
    }
}
