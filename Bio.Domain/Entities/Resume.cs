using Bio.Domain.Enums;

namespace Bio.Domain.Entities
{
    public class Resume
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string ResumeName { get; set; }
        public string Summary { get; set; }
        public string Education { get; set; }
        public string Experience { get; set; }
        public string Skills { get; set; }
        public EducationLevel RequiredEducationLevel { get; set; }
        public string Languages { get; set; }
        public string Hobbies { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public User User { get; set; }
    }
}
