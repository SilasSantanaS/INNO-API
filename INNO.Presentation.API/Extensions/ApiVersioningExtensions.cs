using Microsoft.AspNetCore.Mvc;

namespace INNO.Presentation.API.Extensions
{
    public static class ApiVersioningExtensions
    {
        public static IServiceCollection AddVersioning(this IServiceCollection services)
        {
            services.AddControllersWithViews(o =>
            {
                o.UseGeneralRoutePrefix("api/v{version:apiVersion}");
            });

            services.AddApiVersioning(o =>
            {
                o.DefaultApiVersion = new ApiVersion(1, 0);
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(o =>
            {
                o.GroupNameFormat = "'v'VVV";
                o.SubstituteApiVersionInUrl = true;
            });

            return services;
        }
    }
}
