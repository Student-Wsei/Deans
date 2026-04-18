using AppCore.Dto;
using AppCore.Validators;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AppCore.Module;

public static class StudentModule
{
    public static IServiceCollection AddStudentsModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddValidatorsFromAssemblyContaining<StudentCreateDtoValidator>();
        services.AddScoped<IValidator<StudentCreateDto>, StudentCreateDtoValidator>();
        services.AddScoped<IValidator<StudentUpdateDto>, StudentUpdateDtoValidator>();
        return services;
    }
}
