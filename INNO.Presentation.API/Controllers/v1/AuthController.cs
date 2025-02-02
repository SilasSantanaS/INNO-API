using INNO.Application.Interfaces.Services;
using INNO.Domain.ViewModels.v1;
using INNO.Domain.ViewModels.v1.Auth;
using Microsoft.AspNetCore.Mvc;

namespace INNO.Presentation.API.Controllers.v1
{
    [ApiController]
    [Route("auth")]
    [ApiVersion("1")]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseVM>> Login(
            [FromBody] LoginRequestVM data,
            [FromServices] IAuthService service
        )
        {
            var result = await service.Authenticate(data);

            if (result.Result == null)
            {
                return Unauthorized(new ResponseVM(401, result.Validation.Messages));
            }

            return Ok(result.Result);
        }
    }
}
