namespace Bio.Application.Dtos.JobApplication
{
    public class JobApplicationUpdateDto
    {
        public Guid JobPostId { get; set; }
        public Guid ResumeId { get; set; }
        public string CoverLetter { get; set; }
    }
}
