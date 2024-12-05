using FluentValidation;
using Bio.Application.Dtos.JobApplication;

public class JobApplicationCreateDtoValidator : AbstractValidator<JobApplicationCreateDto>
{
    public JobApplicationCreateDtoValidator()
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
