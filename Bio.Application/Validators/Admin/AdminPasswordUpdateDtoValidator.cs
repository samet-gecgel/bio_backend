using Bio.Application.Dtos.Admin;
using FluentValidation;

namespace Bio.Application.Validators.Admin
{
    public class AdminPasswordUpdateDtoValidator : AbstractValidator<AdminPasswordUpdateDto>
    {
        public AdminPasswordUpdateDtoValidator() 
        {
            RuleFor(a => a.CurrentPassword).NotEmpty().WithMessage("Mevcut şifre alanı boş olamaz.")
                .MinimumLength(6).WithMessage("Parola en az 6 karakter olmalıdır.");

            RuleFor(a => a.NewPassword).NotEmpty().WithMessage("Yeni şifre alanı boş olamaz")
                .MinimumLength(6).WithMessage("Parola en az 6 karakter olmalıdır.");
        }
    }
}
