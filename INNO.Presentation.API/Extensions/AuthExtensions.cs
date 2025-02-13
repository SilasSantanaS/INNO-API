using INNO.Application.Helpers;
using INNO.Domain.Constants;
using INNO.Domain.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace INNO.Presentation.API.Extensions
{
    public static class AuthExtensions
    {
        public static IServiceCollection AddAuth(this IServiceCollection services)
        {
            var key = Encoding.ASCII.GetBytes(JwtHelper.Secret);

            services
            .AddAuthorization(auth =>
            {
                auth.AddPolicy(CustomPolicies.ApplicationManager, policy =>
                {
                    policy.RequireClaim(ClaimTypes.Role, EUserProfile.ApplicationManager.ToString("F"));
                    policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireAuthenticatedUser().Build();
                });

                auth.AddPolicy(CustomPolicies.ApplicationAdmin, policy =>
                {
                    policy.RequireClaim(ClaimTypes.Role, EUserProfile.ApplicationAdmin.ToString("F"));
                    policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireAuthenticatedUser().Build();
                });

                auth.AddPolicy(CustomPolicies.AdminAccess, policy =>
                {
                    policy.RequireClaim(ClaimTypes.Role,
                    [
                        EUserProfile.ApplicationAdmin.ToString("F"),
                        EUserProfile.ApplicationManager.ToString("F")
                    ]);

                    policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireAuthenticatedUser().Build();
                });
            })
            .AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = true;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            return services;
        }

        public static IApplicationBuilder UseAuth(this IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }
    }
}
