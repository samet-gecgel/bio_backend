using Bio.Application.Dtos.Certificate;
using Bio.Application.Dtos.JobApplication;
using Bio.Application.Dtos.Resume;
using Bio.Domain.Entities;
using Bio.Domain.Enums;

namespace Bio.Application.Dtos.User
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string TcKimlik { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime BirthDate { get; set; }
        public string District { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public AccountApprovalStatus ApprovalStatus { get; set; }
        public ICollection<ResumeDto> Resumes { get; set; } = new List<ResumeDto>();
        public ICollection<CertificateDto> Certificates { get; set; } = new List<CertificateDto>();
        public ICollection<JobApplicationDto> JobApplications { get; set; } = new List<JobApplicationDto>();
    }
}
