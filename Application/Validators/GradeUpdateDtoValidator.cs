using AppCore.Dto;
using Domain.Enums;
using FluentValidation;

namespace AppCore.Validators;

public class GradeUpdateDtoValidator : AbstractValidator<GradeUpdateDto>
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

    public GradeUpdateDtoValidator()
    {
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
