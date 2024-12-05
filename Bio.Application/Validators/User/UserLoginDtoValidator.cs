using Bio.Application.Dtos.User;
using FluentValidation;

namespace Bio.Application.Validators.User
{
    public class UserLoginDtoValidator : AbstractValidator<UserLoginDto>
    {
        public UserLoginDtoValidator()
        {
            RuleFor(a => a.Email).NotEmpty().WithMessage("Email alanı boş olamaz.")
                .MaximumLength(100).WithMessage("Email en fazla 100 karakter olabilir")
                .EmailAddress().WithMessage("Girdiğiniz Email doğru formatta değildir.");

            RuleFor(a => a.Password).NotEmpty().WithMessage("Parola alanı boş olamaz")
                .MinimumLength(6).WithMessage("Parola en az 6 karakter olmalıdır");
        }
    }
}
