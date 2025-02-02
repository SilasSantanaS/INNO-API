using INNO.Application.Helpers;
using INNO.Domain.Enums;
using INNO.Domain.Models;
using INNO.Domain.Settings;
using INNO.Infra.Interfaces.Repositories;
using System.Security.Claims;

namespace INNO.Presentation.API.Middlewares
{
    public class CurentUserInjectionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CurentUserInjectionMiddleware> _logger;

        public CurentUserInjectionMiddleware(
            RequestDelegate next,
            ILogger<CurentUserInjectionMiddleware> logger
        )
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var userClaims = context.User;

            var session = context.RequestServices.GetService(typeof(CurrentSession)) as CurrentSession;
            var settings = context.RequestServices.GetService(typeof(TenantPreferences)) as TenantPreferences;
            var userRepository = context.RequestServices.GetService(typeof(IUserRepository)) as IUserRepository;
            var tenantRepository = context.RequestServices.GetService(typeof(ITenantRepository)) as ITenantRepository;

            int.TryParse(userClaims.FindFirstValue(CustomClaims.UserId), out var userId);
            int.TryParse(userClaims.FindFirstValue(CustomClaims.TenantId), out var tenantId);

            if (Enum.TryParse(typeof(EUserProfile), userClaims.FindFirstValue(ClaimTypes.Role), out var profileId))
            {
                session.AccessLevel = (EUserProfile)profileId;
            }

            var user = await userRepository.GetUserById(userId);
            var tenant = await tenantRepository.GetTenantById(tenantId);

            session.UserId = userId;
            session.TenantId = (tenantId == 0) ? null : tenantId;
        }

        private bool ValidateAccess(Tenant tenant, User user)
        {
            if (tenant?.InactivatedAt != null)
            {
                return false;
            }

            if (user?.InactivatedAt != null)
            {
                return false;
            }

            return true;
        }
    }
}
