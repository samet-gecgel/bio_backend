using Bio.Application.Dtos.Certificate;
using Bio.Domain.Enums;

namespace Bio.Application.Dtos.JobApplication
{
    public class JobApplicationDetailDto
    {
        public Guid Id { get; set; }
        public string JobTitle { get; set; }
        public string ApplicantName { get; set; }
        public DateTime ApplicationDate { get; set; }
        public ApplicationStatus Status { get; set; }
        public string CoverLetter { get; set; }
        public Guid? ResumeId { get; set; }
        public List<CertificateDto> Certificates { get; set; }
    }

}
