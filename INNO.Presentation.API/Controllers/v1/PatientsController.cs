using INNO.Application.Interfaces.Services;
using INNO.Domain.Constants;
using INNO.Domain.ViewModels.v1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using INNO.Domain.ViewModels.v1.Patients;
using INNO.Domain.Filters;

namespace INNO.Presentation.API.Controllers.v1
{
    [Authorize]
    [ApiController]
    [ApiVersion("1")]
    [Route("patients")]
    public class PatientsController : ControllerBase
    {
        [HttpPost("")]
        [Authorize(CustomPolicies.ApplicationAdmin)]
        public async Task<ActionResult<PatientResponseVM>> CreatePatient(
            [FromBody] PatientRequestVM data,
            [FromServices] IPatientService service
        )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (result, validation) = await service.CreatePatient(data);

            if (result == null)
            {
                return BadRequest(new ResponseVM(400, validation.Messages));
            }

            return Ok(result);
        }

        [HttpGet("")]
        [Authorize(CustomPolicies.ApplicationAdmin)]
        public async Task<ActionResult<PatientResponseVM>> ListPPatients(
            [FromQuery] PatientFilter filter,
            [FromServices] IPatientService service
        )
        {
            return Ok(await service.ListPatients(filter));
        }

        [HttpGet("{id}")]
        [Authorize(CustomPolicies.ApplicationAdmin)]
        public async Task<ActionResult<PatientResponseVM>> PatientById(
            int id,
            [FromServices] IPatientService service
        )
        {
            var result = await service.GetPatientById(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(CustomPolicies.ApplicationAdmin)]
        public async Task<ActionResult<PatientResponseVM>> UpdatePatient(
            int id,
            [FromBody] PatientRequestVM data,
            [FromServices] IPatientService service
        )
        {
            var result = await service.UpdatePatient(id, data);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost("{id}/activate")]
        [Authorize(CustomPolicies.ApplicationAdmin)]
        public async Task<ActionResult> ActivatePatient(
            int id,
            [FromServices] IPatientService service
        )
        {
            var (result, _) = await service.ActivatePatient(id);

            if (result)
            {
                return NoContent();
            }

            return BadRequest();
        }

        [HttpDelete("{id}")]
        [Authorize(CustomPolicies.ApplicationAdmin)]
        public async Task<ActionResult> InactivatePatient(
            int id,
            [FromServices] IPatientService service
        )
        {
            var (result, _) = await service.InactivatePatient(id);

            if (result)
            {
                return NoContent();
            }

            return BadRequest();
        }
    }
}
