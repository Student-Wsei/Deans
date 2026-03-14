using Domain;
using Domain.Entities;
using Infrastructure.Repositories;

namespace Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddAuthorization();

        builder.Services.AddSingleton<IStudentRepository, InMemoryStudentRepository>();
        builder.Services.AddSingleton<ILecturerRepository, InMemoryLecturerRepository>();
        builder.Services.AddSingleton<IGradeRepository, InMemoryGradeRepository>();
        builder.Services.AddSingleton<ICourseRepository, InMemoryCourseRepository>();
        builder.Services.AddSingleton<IDegreeProgramRepository, InMemoryDegreeProgramRepository>();

        builder.Services.AddOpenApi();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();

        app.Run();
    }
}
