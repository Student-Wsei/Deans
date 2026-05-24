using System;
using AppCore.Authorization;
using AppCore.Services;
using Application.Services;
using Domain;
using Infrastructure.EntityFramework.Context;
using Infrastructure.EntityFramework.Entities;
using Infrastructure.EntityFramework.Repositories;
using Infrastructure.EntityFramework.UnitOfWork;
using Infrastructure.Repositories;
using Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

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
                configuration.GetConnectionString("UniversityDb"))
            .ConfigureWarnings(w =>
                w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning)));

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
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IDataSeeder, UniversityDbSeeder>();

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

    public static IServiceCollection AddJwt(this IServiceCollection services, JwtSettings jwtOptions)
    {
        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = jwtOptions.GetSymmetricKey(),
                    ClockSkew = TimeSpan.Zero
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy(AppPolicies.AdminOnly.ToString(), policy =>
                policy.RequireRole(UserRole.Administrator.ToString()));

            options.AddPolicy(AppPolicies.ActiveUser.ToString(), policy =>
                policy
                    .RequireAuthenticatedUser()
                    .RequireClaim("status", SystemUserStatus.Active.ToString()));

            options.AddPolicy(AppPolicies.SalesDepartment.ToString(), policy =>
                policy.RequireClaim("department", "Sales"));

            options.DefaultPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        });

        return services;
    }
}
