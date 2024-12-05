using Bio.Domain.Enums;

namespace Bio.Domain.Entities
{
    public class JobApplication
    {
        public Guid Id { get; set; }
        public Guid JobPostId { get; set; }
        public JobPost JobPost { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid? ResumeId { get; set; }
        public Resume Resume { get; set; }

        public string CoverLetter { get; set; }
        public DateTime ApplicationDate { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public ApplicationStatus Status { get; set; } 
        
    }
}
