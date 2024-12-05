namespace Bio.Application.Dtos.JobApplication
{
    public class JobApplicationCreateDto
    {
        public Guid JobPostId { get; set; }
        public Guid ResumeId { get; set; }
        public string CoverLetter { get; set; }
    }
}
