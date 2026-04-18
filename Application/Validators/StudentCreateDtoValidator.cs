using AppCore.Dto;
using FluentValidation;

namespace AppCore.Validators;

public class StudentCreateDtoValidator : AbstractValidator<StudentCreateDto>
{
    public StudentCreateDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Imię jest wymagane.")
            .MaximumLength(100).WithMessage("Imię nie może przekraczać 100 znaków.")
            .Matches(@"^[\p{L}\s\-]+$").WithMessage("Imię zawiera niedozwolone znaki.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Nazwisko jest wymagane.")
            .MaximumLength(200).WithMessage("Nazwisko nie może przekraczać 200 znaków.")
            .Matches(@"^[\p{L}\s\-]+$").WithMessage("Nazwisko zawiera niedozwolone znaki.");

        RuleFor(x => x.NationalId)
            .NotEmpty().WithMessage("PESEL jest wymagany.")
            .Length(11).WithMessage("PESEL musi mieć 11 znaków.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email jest wymagany.")
            .EmailAddress().WithMessage("Nieprawidłowy format adresu email.")
            .MaximumLength(200);

        RuleFor(x => x.YearOfStudy)
            .Must(year => year >= 1 && year <= 5)
            .WithMessage("Niepoprawny rok studiów.");

        RuleFor(x => x.StudentId)
            .NotEmpty().WithMessage("Numer albumu jest wymagany.")
            .MaximumLength(20);

        RuleFor(x => x.ProgramCode)
            .NotEmpty().WithMessage("Kod kierunku jest wymagany.");

        RuleFor(x => x.EnrollmentYearFrom)
            .GreaterThan(2000).WithMessage("Rok rozpoczęcia musi być większy od 2000.");
    }
}
