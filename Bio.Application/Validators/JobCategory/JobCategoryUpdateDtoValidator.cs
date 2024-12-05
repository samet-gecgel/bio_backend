using Bio.Application.Dtos.JobCategory;
using FluentValidation;

namespace Bio.Application.Validators.JobCategory
{
    public class JobCategoryUpdateDtoValidator : AbstractValidator<JobCategoryUpdateDto>
    {
        public JobCategoryUpdateDtoValidator() 
        {
            RuleFor(jc => jc.Name).NotEmpty().WithMessage("Kategori adı boş olamaz")
                .MaximumLength(100).WithMessage("Kategori adı en fazla 100 karakter olmalıdır.");
        }
    }
}
