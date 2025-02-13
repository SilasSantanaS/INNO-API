using INNO.Application.Interfaces.Services;
using INNO.Domain.Constants;
using INNO.Domain.ViewModels.v1.Preferences;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace INNO.Presentation.API.Controllers.v1
{
    [ApiController]
    [ApiVersion("1")]
    [Route("preferences/parameters")]
    [Authorize(CustomPolicies.ApplicationAdmin)]

    public class PreferencesController : ControllerBase
    {
        [HttpGet("")]
        public async Task<ActionResult<InnoSettingsResponseVM>> GetPreferences(
            [FromServices] IPreferencesService service
        )
        {
            return Ok(await service.GetPreferences());
        }

        [HttpPut("")]
        public async Task<ActionResult<InnoSettingsResponseVM>> UpdatePreferences(
            [FromBody] InnoSettingsRequestVM data,
            [FromServices] IPreferencesService service
        )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await service.UpdatePreferences(data);

            if (result.Success)
            {
                return Ok(result.Result);
            }

            return BadRequest(result);
        }

        [HttpGet("options")]
        public async Task<ActionResult<PreferencesOptionsResponseVM>> GetOptions(
            [FromServices] IPreferencesService service
        )
        {
            return Ok(await service.ListPreferencesOptions());
        }
    }
}
