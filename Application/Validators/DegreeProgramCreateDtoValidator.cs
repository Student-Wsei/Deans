using AppCore.Dto;
using Domain.Enums;
using FluentValidation;

namespace AppCore.Validators;

public class DegreeProgramCreateDtoValidator : AbstractValidator<DegreeProgramCreateDto>
{
    public DegreeProgramCreateDtoValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Kod kierunku jest wymagany.")
            .MaximumLength(20);

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nazwa kierunku jest wymagana.")
            .MaximumLength(200);

        RuleFor(x => x.Faculty)
            .NotEmpty().WithMessage("Wydział jest wymagany.")
            .MaximumLength(200);

        RuleFor(x => x.DegreeType)
            .IsInEnum().WithMessage("Niepoprawny typ studiów.");

        RuleFor(x => x.DurationYears)
            .GreaterThan(0).WithMessage("Czas trwania musi być większy od 0.")
            .LessThanOrEqualTo(10);

        RuleFor(x => x.MinEctsForDiploma)
            .GreaterThan(0).WithMessage("Minimalna liczba ECTS musi być większa od 0.")
            .LessThanOrEqualTo(360);
    }
}
