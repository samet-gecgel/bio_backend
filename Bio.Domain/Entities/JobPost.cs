using Bio.Domain.Enums;

namespace Bio.Domain.Entities
{
    public class JobPost
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal? MinSalary { get; set; }
        public decimal? MaxSalary { get; set; }
        public DateTime PublishedDate { get; set; }
        public DateTime ApplicationDeadline { get; set; }
        public string District { get; set; }
        public string Benefits { get; set; }
        public WeekDay OffDays { get; set; }
        public bool? RequiresDrivingLicense { get; set; }
        public string MinAge { get; set; }
        public string MaxAge { get; set; }
        public int MinExperienceYears { get; set; }
        public EducationLevel RequiredEducationLevel { get; set; }
        public ExperienceLevel ExperienceLevel { get; set; }
        public bool IsDisabledFriendly { get; set; }
        public ApplicationStatus ApplicationStatus { get; set; }

        public int ViewCount { get; set; }
        public Guid? CategoryId { get; set; }
        public JobCategory JobCategory { get; set; }

        public Guid CompanyId { get; set; }
        public Company Company { get; set; }

        public JobType JobType { get; set; }

        public bool IsActive { get; set; }

        public ICollection<JobApplication> JobApplications { get; set; } = new List<JobApplication>();


    }
}
