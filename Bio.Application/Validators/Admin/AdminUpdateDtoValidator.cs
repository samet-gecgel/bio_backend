using Bio.Application.Dtos.Admin;
using FluentValidation;

namespace Bio.Application.Validators.Admin
{
    public class AdminUpdateDtoValidator : AbstractValidator<AdminUpdateDto>
    {
        public AdminUpdateDtoValidator() 
        {
            RuleFor(a => a.FullName).NotEmpty().WithMessage("Admin isim ve soyisim alanı boş olamaz.")
                .MaximumLength(100).WithMessage("İsim ve Soyisim en fazla 100 karakter olabilir");

            RuleFor(a => a.Email).NotEmpty().WithMessage("Email alanı boş olamaz.")
                .MaximumLength(100).WithMessage("Email en fazla 100 karakter olabilir")
                .EmailAddress().WithMessage("Girdiğiniz Email doğru formatta değildir.");

            RuleFor(a => a.Department).NotEmpty().WithMessage("Departman alanı boş olamaz.")
                .MaximumLength(100).WithMessage("Departman en fazla 100 karakter olabilir");
        }
    }
}
