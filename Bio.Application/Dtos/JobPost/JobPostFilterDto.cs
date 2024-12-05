using Bio.Domain.Enums;

namespace Bio.Application.Dtos.JobPost
{
    public class JobPostFilterDto
    {
        public string? Title { get; set; }
        public List<string>? Districts { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? CategoryName { get; set; }
        public string? CompanyName { get; set; }
        public WeekDay? OffDays { get; set; }
        public EducationLevel? RequiredEducationLevel { get; set; }
        public JobType? JobTypes { get; set; } 
        public ExperienceLevel? ExperienceLevel { get; set; }
        public bool? RequiresDrivingLicense { get; set; }
        public bool? IsDisabledFriendly { get; set; }
        public bool? IsActive { get; set; }
        public ApplicationStatus ApplicationStatus { get; set; }
    }
}
