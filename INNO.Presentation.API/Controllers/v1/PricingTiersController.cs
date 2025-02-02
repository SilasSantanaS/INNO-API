using INNO.Domain.ViewModels.v1;
using Microsoft.AspNetCore.Mvc;

namespace INNO.Presentation.API.Controllers.v1
{
    [ApiController]
    [ApiVersion("1")]
    [Route("pricing_tiers")]
    public class PricingTiersController : ControllerBase
    {
        [HttpGet("")]
        public async Task<ActionResult> ListTiers()
        {
            return Ok(new ListResponseVM<object>()
            {
                Metadata = new ListMetaVM()
                {
                    Count = 3,
                    Page = 1,
                    PageLimit = 3,
                    TotalItems = 3,
                },
                Result = [
                    new {
                        id = 1,
                        name = "Starter",
                    },
                    new {
                        id = 2,
                        name = "Pro",
                    },
                    new {
                        id = 3,
                        name = "Enterprise",
                    },
                ]
            });
        }
    }
}
