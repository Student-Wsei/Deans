using System;
using Application.Services;
using Domain;
using Infrastructure.EntityFramework.Context;
using Infrastructure.EntityFramework.Entities;
using Infrastructure.EntityFramework.Repositories;
using Infrastructure.EntityFramework.UnitOfWork;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.EntityFramework;

public static class UniversityOfficeInfrastructureModule
{
    public static IServiceCollection AddUniversityOfficeEfModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IStudentRepository, EfStudentRepository>();
        services.AddScoped<ILecturerRepository, EfLecturerRepository>();
        services.AddScoped<IGradeRepository, EfGradeRepository>();
        services.AddScoped<ICourseRepository, EfCourseRepository>();
        services.AddScoped<IDegreeProgramRepository, EfDegreeProgramRepository>();

        services.AddScoped<IUniversityUnitOfWork, EfUniversityUnitOfWork>();

        services.AddDbContext<UniversityOfficeDbContext>(options =>
            options.UseSqlite(
                configuration.GetConnectionString("UniversityDb")));

        services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            })
            .AddEntityFrameworkStores<UniversityOfficeDbContext>()
            .AddDefaultTokenProviders();

        services.AddScoped<IStudentService, StudentService>();
        services.AddScoped<IDegreeProgramService, DegreeProgramService>();

        return services;
    }

    public static IServiceCollection AddUniversityOfficeMemoryModule(
        this IServiceCollection services)
    {
        services.AddSingleton<IStudentRepository, MemoryStudentRepository>();
        services.AddSingleton<ILecturerRepository, MemoryLecturerRepository>();
        services.AddSingleton<IGradeRepository, InMemoryGradeRepository>();
        services.AddSingleton<ICourseRepository, InMemoryCourseRepository>();
        services.AddSingleton<IDegreeProgramRepository, InMemoryDegreeProgramRepository>();

        services.AddSingleton<IUniversityUnitOfWork, MemoryUniversityUnitOfWork>();

        services.AddSingleton<IStudentService, StudentService>();
        services.AddSingleton<IDegreeProgramService, DegreeProgramService>();

        return services;
    }
}
