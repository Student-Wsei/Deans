using System.Linq;
using System.Threading.Tasks;
using AppCore.Module;
using FluentValidation.AspNetCore;
using Infrastructure.EntityFramework;
using Infrastructure.Security;
using Microsoft.Extensions.DependencyInjection;

namespace Api;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddFluentValidationAutoValidation();

        builder.Services.AddStudentsModule(builder.Configuration);
        builder.Services.AddUniversityOfficeEfModule(builder.Configuration);
        //builder.Services.AddUniversityOfficeMemoryModule();

        var jwtSettings = new JwtSettings(builder.Configuration);
        builder.Services.AddSingleton(jwtSettings);
        builder.Services.AddJwt(jwtSettings);

        builder.Services.AddExceptionHandler<ProblemDetailsExceptionHandler>();
        builder.Services.AddProblemDetails();

        builder.Services.AddOpenApi();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();

            using var scope = app.Services.CreateScope();
            var seeders = scope.ServiceProvider
                .GetServices<IDataSeeder>()
                .OrderBy(s => s.Order);

            foreach (var seeder in seeders)
            {
                await seeder.SeedAsync();
            }
        }

        app.UseExceptionHandler();
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}
