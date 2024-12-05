using Bio.Application.Dtos.Company;
using FluentValidation;

namespace Bio.Application.Validators.Company
{
    public class CompanyLoginDtoValidator : AbstractValidator<CompanyLoginDto>
    {
        public CompanyLoginDtoValidator()
        {
            RuleFor(a => a.Email).NotEmpty().WithMessage("Email alanı boş olamaz.")
            .MaximumLength(100).WithMessage("Email en fazla 100 karakter olabilir")
            .EmailAddress().WithMessage("Girdiğiniz Email doğru formatta değildir.");

            RuleFor(a => a.Password).NotEmpty().WithMessage("Parola alanı boş olamaz")
                .MinimumLength(6).WithMessage("Parola en az 6 karakter olmalıdır");
        }
    }
}
