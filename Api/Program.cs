using AppCore.Module;
using Application.Services;
using Domain;
using Domain.Entities;
using FluentValidation.AspNetCore;
using Infrastructure.Repositories;
using Api.Controllers;

namespace Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddFluentValidationAutoValidation();

        builder.Services.AddStudentsModule(builder.Configuration);

        builder.Services.AddSingleton<IStudentRepository, MemoryStudentRepository>();
        builder.Services.AddSingleton<ILecturerRepository, MemoryLecturerRepository>();
        builder.Services.AddSingleton<IGradeRepository, InMemoryGradeRepository>();
        builder.Services.AddSingleton<ICourseRepository, InMemoryCourseRepository>();
        builder.Services.AddSingleton<IDegreeProgramRepository, InMemoryDegreeProgramRepository>();

        builder.Services.AddSingleton<IUniversityUnitOfWork, MemoryUniversityUnitOfWork>();
        builder.Services.AddSingleton<IStudentService, MemoryStudentService>();

        builder.Services.AddOpenApi();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}
