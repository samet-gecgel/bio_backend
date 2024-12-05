using Bio.Domain.Enums;

namespace Bio.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string TcKimlik { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime BirthDate { get; set; }
        public string District { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public UserRole Role { get; set; }
        public AccountApprovalStatus ApprovalStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public ICollection<Resume> Resumes { get; set; } = new List<Resume>();
        public ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();
        public ICollection<JobApplication> JobApplications { get; set; } = new List<JobApplication>();


    }
}
