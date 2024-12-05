using Bio.Domain.Enums;

namespace Bio.Application.Dtos.Resume
{
    public class ResumeCreateDto
    {
        public string ResumeName { get; set; }
        public string Summary { get; set; }
        public string Education { get; set; }
        public string Experience { get; set; }
        public string Skills { get; set; }
        public string Languages { get; set; }
        public EducationLevel RequiredEducationLevel { get; set; }
        public string Hobbies { get; set; }

    }
}
