using Bio.Application.Dtos.JobApplication;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bio.Application.Validators.JobApplication
{
    public class JobApplicationUpdateDtoValidator : AbstractValidator<JobApplicationUpdateDto>
    {
        public JobApplicationUpdateDtoValidator() 
        {
            RuleFor(j => j.JobPostId)
                .NotEmpty().WithMessage("JobPostId alanı boş olamaz.");

            RuleFor(j => j.ResumeId)
                .NotEmpty().WithMessage("Özgeçmiş alanı boş olamaz.");

            RuleFor(j => j.CoverLetter)
                .NotEmpty().WithMessage("Ön yazı alanı boş olamaz.")
                .MaximumLength(1000).WithMessage("Ön yazı en fazla 1000 karakter olabilir.");
        }
    }
}
