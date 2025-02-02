using FluentMigrator.Runner;
using INNO.Application.Interfaces.Services;
using INNO.Application.Services;
using INNO.Domain.Mappings;
using INNO.Domain.Models;
using INNO.Domain.Settings;
using INNO.Infra.Interfaces;
using INNO.Infra.Interfaces.Repositories;
using INNO.Infra.Migrations;
using INNO.Infra.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace INNO.Infra.IOC
{
    public static class Resolver
    {
        public static IServiceCollection AddSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DbSettings>(configuration.GetSection("ConnectionStrings"));

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ITenantService, TenantService>();
            services.AddTransient<IPatientService, PatientService>();
            services.AddTransient<IProfessionalService, ProfessionalService>();

            services.AddScoped<CurrentSession>();

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

            string connectionString = configuration.GetConnectionString("PgConnection");
            var connectionFactory = new PgConnectionFactory(connectionString);

            Console.WriteLine($"[PgConnection:{connectionString}]");

            services.AddFluentMigratorCore().ConfigureRunner(rb => 
                rb.AddPostgres()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(typeof(MigrationsAssembly).Assembly)
                .For.Migrations())
                .AddLogging(lb => lb.AddFluentMigratorConsole());

            services.AddSingleton<IDbConnectionFactory>(connectionFactory);
            services.AddScoped<DbSession>();

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ITenantRepository, TenantRepository>();
            services.AddTransient<IPatientRepository, PatientRepository>();
            services.AddTransient<IProfessionalRepository, ProfessionalRepository>();

            return services;
        }

        public static IServiceCollection AddMappings(this IServiceCollection services)
        {
            services.AddAutoMapper(ps =>
            {
                ps.AddProfile(new UserMappingProfile());
                ps.AddProfile(new TenantMappingProfile());
                ps.AddProfile(new PatientMappingProfile());
                ps.AddProfile(new ProfessionalMappingProfile());
            });

            return services;
        }

        public static IServiceCollection ResolveDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMappings();
            services.AddServices();
            services.AddSettings(configuration);
            services.AddRepositories(configuration);

            return services;
        }
    }
}
