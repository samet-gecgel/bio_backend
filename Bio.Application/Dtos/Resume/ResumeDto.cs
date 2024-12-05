using Bio.Application.Dtos.Certificate;
using Bio.Domain.Enums;

namespace Bio.Application.Dtos.Resume
{
    public class ResumeDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string ResumeName { get; set; }
        public string Summary { get; set; }
        public string Education { get; set; }
        public string Experience { get; set; }
        public string Skills { get; set; }
        public string Languages { get; set; }
        public EducationLevel RequiredEducationLevel { get; set; }
        public string Hobbies { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
}
