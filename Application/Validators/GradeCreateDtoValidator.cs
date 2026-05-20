using AppCore.Dto;
using Domain.Enums;
using FluentValidation;

namespace AppCore.Validators;

public class GradeCreateDtoValidator : AbstractValidator<GradeCreateDto>
{
    private static readonly HashSet<GradeValue> Allowed = new()
    {
        GradeValue.Grade20,
        GradeValue.Grade30,
        GradeValue.Grade35,
        GradeValue.Grade40,
        GradeValue.Grade45,
        GradeValue.Grade50
    };

    public GradeCreateDtoValidator()
    {
        RuleFor(x => x.CourseId)
            .NotEqual(Guid.Empty).WithMessage("Identyfikator kursu jest wymagany.");

        RuleFor(x => x.LecturerId)
            .NotEqual(Guid.Empty).WithMessage("Identyfikator prowadzącego jest wymagany.");

        RuleFor(x => x.AcademicYearId)
            .NotEqual(Guid.Empty).WithMessage("Identyfikator roku akademickiego jest wymagany.");

        RuleFor(x => x.Date)
            .NotEqual(default(DateTime)).WithMessage("Data jest wymagana.")
            .Must(d => d.Date <= DateTime.UtcNow.Date)
            .WithMessage("Data nie może być z przyszłości.");

        RuleFor(x => x.GradeType)
            .IsInEnum().WithMessage("Nieprawidłowy typ oceny.");

        RuleFor(x => x.GradeValue)
            .Must(v => Allowed.Contains(v))
            .WithMessage("Nieprawidłowa wartość oceny.");
    }
}
