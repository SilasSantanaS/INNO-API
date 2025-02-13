using FluentMigrator.Runner;
using INNO.Infra.IOC;
using INNO.Presentation.API.Extensions;
using INNO.Presentation.API.Middlewares;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var configuration = builder.Configuration;

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddRateLimiting(configuration);
        builder.Services.AddVersioning();
        builder.Services.AddAuth();
        builder.Services.AddSwagger();
        builder.Services.ResolveDependencies(configuration);

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var serviceProvider = scope.ServiceProvider;

            UpdateDatabase(serviceProvider);
        }

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        //app.UseHttpsRedirection();

        app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

        app.UseAuth();

        app.UseMiddleware<CurentUserInjectionMiddleware>();

        app.MapControllers();

        app.Run();
    }

    private static void UpdateDatabase(IServiceProvider serviceProvider)
    {
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

        runner.MigrateUp();
    }
}