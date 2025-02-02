using INNO.Application.Interfaces.Services;
using INNO.Domain.Constants;
using INNO.Domain.Filters;
using INNO.Domain.ViewModels.v1;
using INNO.Domain.ViewModels.v1.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace INNO.Presentation.API.Controllers.v1
{
    [ApiController]
    [Route("users")]
    [ApiVersion("1")]
    public class UsersController : ControllerBase
    {
        [HttpGet("")]
        [Authorize(CustomPolicies.AdminAccess)]
        public async Task<ActionResult<UserListResponseVM>> ListUsers(
            [FromQuery] UserFilter filter,
            [FromServices] IUserService service
        )
        {
            return Ok(await service.ListUsers(filter));
        }

        [HttpGet("{id}")]
        [Authorize(CustomPolicies.AdminAccess)]
        public async Task<ActionResult<UserResponseVM>> GetUserById(
            int id,
            [FromServices] IUserService service
        )
        {
            var result = await service.GetUserById(id);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost("")]
        [Authorize(CustomPolicies.AdminAccess)]
        public async Task<ActionResult<UserResponseVM>> CreateUser(
            [FromBody] UserRequestVM data,
            [FromServices] IUserService service
        )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await service.CreateUser(data);

            if (result.User is null)
            {
                return BadRequest(new ResponseVM(400, result.Validation.Messages));
            }

            return Ok(result.User);
        }

        [HttpPut("{id}")]
        [Authorize(CustomPolicies.AdminAccess)]
        public async Task<ActionResult<UserResponseVM>> UpdateUser(
            int id,
            [FromBody] UserPutRequestVM data,
            [FromServices] IUserService service
        )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await service.UpdateUser(id, data);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(CustomPolicies.AdminAccess)]
        public async Task<ActionResult<ResponseVM>> DeleteUser(
            int id,
            [FromQuery] int tenantId,
            [FromServices] IUserService service
        )
        {
            var result = await service.DeleteUser(id, tenantId);

            if (result.Item1)
            {
                return Ok(new ResponseVM(200, result.Item2));
            }

            return BadRequest(new ResponseVM(400, result.Item2));
        }
    }
}
