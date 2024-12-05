using Bio.Application.Dtos.Certificate;
using FluentValidation;

namespace Bio.Application.Validators.Certificate
{
    public class CertificateUpdateDtoValidator : AbstractValidator<CertificateUpdateDto>
    {
        public CertificateUpdateDtoValidator() 
        {
            RuleFor(c => c.CertificateName).NotEmpty().WithMessage("Sertifika adı alanı boş olamaz.")
                .MaximumLength(500).WithMessage("Sertifika adı en fazla 500 karakter olmalıdır.");

            RuleFor(c => c.Institution).NotEmpty().WithMessage("Kurum Alanı boş olamaz.")
                .MaximumLength(100).WithMessage("Kurum adı en fazla 100 karakter olabilir");

            RuleFor(c => c.IssueDate).NotEmpty().WithMessage("Sertifika veriliş tarihi boş olamaz");
        }
    }
}