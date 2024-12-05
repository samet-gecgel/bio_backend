using Bio.Application.Dtos.Certificate;
using Bio.Domain.Enums;

namespace Bio.Application.Dtos.JobApplication
{
    public class JobApplicationDto
    {
        public Guid Id { get; set; }
        public string JobPostTitle { get; set; }
        public string CompanyName { get; set; }
        public string ApplicantName { get; set; }
        public string ResumeId { get; set; }
        public DateTime ApplicationDate { get; set; }
        public ApplicationStatus Status { get; set; }
    }
}
