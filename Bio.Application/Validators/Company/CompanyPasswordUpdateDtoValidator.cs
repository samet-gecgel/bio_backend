using FluentValidation;
using Bio.Application.Dtos.Company;

namespace Bio.Application.Validators.Company
{
    public class CompanyPasswordUpdateDtoValidator : AbstractValidator<CompanyPasswordUpdateDto>
    {
        public CompanyPasswordUpdateDtoValidator()
        {
            RuleFor(c => c.CurrentPassword).NotEmpty().WithMessage("Mevcut şifre alanı boş olamaz.")
                .MinimumLength(6).WithMessage("Parola en az 6 karakter olmalıdır.");

            RuleFor(c => c.NewPassword).NotEmpty().WithMessage("Yeni şifre alanı boş olamaz")
                .MinimumLength(6).WithMessage("Parola en az 6 karakter olmalıdır.");
        }
    }
}