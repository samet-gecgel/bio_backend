using FluentValidation;
using Bio.Application.Dtos.User;

namespace Bio.Application.Validators.User
{
    public class UserPasswordUpdateDtoValidator : AbstractValidator<UserPasswordUpdateDto>
    {
        public UserPasswordUpdateDtoValidator()
        {
            RuleFor(u => u.CurrentPassword)
                .NotEmpty().WithMessage("Mevcut şifre boş olamaz.")
                .MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalıdır.");

            RuleFor(u => u.NewPassword)
                .NotEmpty().WithMessage("Yeni şifre boş olamaz.")
                .MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalıdır.")
                .NotEqual(u => u.CurrentPassword).WithMessage("Yeni şifre, mevcut şifre ile aynı olamaz.");
        }
    }
}