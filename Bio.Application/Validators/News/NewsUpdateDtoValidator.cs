using Bio.Application.Dtos.News;
using FluentValidation;

namespace Bio.Application.Validators.News
{
    public class NewsUpdateDtoValidator : AbstractValidator<NewsUpdateDto>
    {
        public NewsUpdateDtoValidator() 
        {
            RuleFor(n => n.Title).NotEmpty().WithMessage("Haber Başlığı boş olamaz.")
                .MaximumLength(1000).WithMessage("Haber başlığı en fazla 1000 karakter olmalıdır.");

            RuleFor(n => n.Description).NotEmpty().WithMessage("Haber Açıklaması boş olamaz.");

        }
    }
}
