using AppCore.Module;
using FluentValidation.AspNetCore;
using Infrastructure.EntityFramework;

namespace Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddFluentValidationAutoValidation();

        builder.Services.AddStudentsModule(builder.Configuration);
        builder.Services.AddUniversityOfficeEfModule(builder.Configuration);
        builder.Services.AddUniversityOfficeMemoryModule();

        builder.Services.AddExceptionHandler<ProblemDetailsExceptionHandler>();
        builder.Services.AddProblemDetails();

        builder.Services.AddOpenApi();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseExceptionHandler();
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}
