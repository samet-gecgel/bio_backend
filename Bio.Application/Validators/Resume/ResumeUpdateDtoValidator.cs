using FluentValidation;
using Bio.Application.Dtos.Resume;

public class ResumeUpdateDtoValidator : AbstractValidator<ResumeUpdateDto>
{
    public ResumeUpdateDtoValidator()
    {
        RuleFor(r => r.ResumeName)
            .NotEmpty().WithMessage("ÖzGeçmiş adı boş olamaz.")
            .MaximumLength(500).WithMessage("ÖzGeçmiş adı en fazla 500 karakter olabilir.");

        RuleFor(r => r.Summary)
            .NotEmpty().WithMessage("Özet alanı boş olamaz.")
            .MaximumLength(1000).WithMessage("Özet en fazla 1000 karakter olabilir.");

        RuleFor(r => r.Education)
            .NotEmpty().WithMessage("Eğitim bilgisi boş olamaz.")
            .MaximumLength(1000).WithMessage("Eğitim bilgisi en fazla 1000 karakter olabilir.");

        RuleFor(r => r.Experience)
            .NotEmpty().WithMessage("Deneyim bilgisi boş olamaz.")
            .MaximumLength(1000).WithMessage("Deneyim bilgisi en fazla 1000 karakter olabilir.");

        RuleFor(r => r.Skills)
            .NotEmpty().WithMessage("Yetenekler alanı boş olamaz.")
            .MaximumLength(500).WithMessage("Yetenekler en fazla 500 karakter olabilir.");

        RuleFor(r => r.Languages)
            .NotEmpty().WithMessage("Diller alanı boş olamaz.")
            .MaximumLength(500).WithMessage("Diller en fazla 500 karakter olabilir.");

        RuleFor(r => r.Hobbies)
            .MaximumLength(500).WithMessage("Hobiler en fazla 500 karakter olabilir.");

    }
}
