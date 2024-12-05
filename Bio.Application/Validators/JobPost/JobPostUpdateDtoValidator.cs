using FluentValidation;
using Bio.Application.Dtos.JobPost;
using Bio.Domain.Enums;

public class JobPostUpdateDtoValidator : AbstractValidator<JobPostUpdateDto>
{
    public JobPostUpdateDtoValidator()
    {
        RuleFor(j => j.Title)
            .NotEmpty().WithMessage("Başlık alanı boş olamaz.")
            .MaximumLength(100).WithMessage("Başlık en fazla 100 karakter olabilir.");

        RuleFor(j => j.Description)
            .NotEmpty().WithMessage("Açıklama alanı boş olamaz.")
            .MaximumLength(2000).WithMessage("Açıklama en fazla 2000 karakter olabilir.");

        RuleFor(j => j.MinSalary)
            .GreaterThanOrEqualTo(0).When(j => j.MinSalary.HasValue)
            .WithMessage("Minimum maaş 0'dan küçük olamaz.");

        RuleFor(j => j.MaxSalary)
            .GreaterThan(j => j.MinSalary).When(j => j.MaxSalary.HasValue && j.MinSalary.HasValue)
            .WithMessage("Maksimum maaş, minimum maaştan büyük olmalıdır.");

        RuleFor(j => j.ApplicationDeadline)
            .NotEmpty().WithMessage("Başvuru son tarihi alanı boş olamaz.")
            .GreaterThan(DateTime.UtcNow).WithMessage("Başvuru son tarihi bugünden ileri bir tarih olmalıdır.");

        RuleFor(j => j.District)
            .NotEmpty().WithMessage("İlçe bilgisi alanı boş olamaz.")
            .MaximumLength(100).WithMessage("İlçe en fazla 100 karakter olabilir.");

        RuleFor(j => j.Benefits)
            .MaximumLength(500).WithMessage("Yan haklar en fazla 500 karakter olabilir.");

        RuleFor(j => j.OffDays)
           .NotEqual(WeekDay.None).WithMessage("En az bir tatil günü seçilmelidir.");

        RuleFor(j => j.MinAge)
            .Matches(@"^\d+$").When(j => !string.IsNullOrEmpty(j.MinAge))
            .WithMessage("Minimum yaş geçerli bir sayı olmalıdır.");

        RuleFor(j => j.MaxAge)
            .Matches(@"^\d+$").When(j => !string.IsNullOrEmpty(j.MaxAge))
            .WithMessage("Maksimum yaş geçerli bir sayı olmalıdır.")
            .Must((dto, maxAge) => string.IsNullOrEmpty(dto.MinAge) || int.Parse(maxAge) > int.Parse(dto.MinAge))
            .When(j => !string.IsNullOrEmpty(j.MaxAge) && !string.IsNullOrEmpty(j.MinAge))
            .WithMessage("Maksimum yaş, minimum yaştan büyük olmalıdır.");

        RuleFor(j => j.MinExperienceYears)
            .GreaterThanOrEqualTo(0).WithMessage("Minimum tecrübe yılı negatif olamaz.");

        RuleFor(j => j.RequiredEducationLevel)
            .NotEmpty().WithMessage("En az bir eğitim seviyesi seçilmelidir.");

        RuleFor(j => j.CategoryId)
            .NotEmpty().WithMessage("Kategori ID'si alanı boş olamaz.");

        RuleFor(j => j.CompanyId)
            .NotEmpty().WithMessage("Şirket ID'si alanı boş olamaz.");

        RuleFor(j => j.JobType)
            .IsInEnum().WithMessage("Geçerli bir iş türü seçilmelidir.");
    }
}
