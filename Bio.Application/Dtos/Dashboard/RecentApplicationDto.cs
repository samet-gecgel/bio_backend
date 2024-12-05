namespace Bio.Application.Dtos.Dashboard
{
    public class RecentApplicationDto
    {
        public Guid Id { get; set; }
        public string JobPostTitle { get; set; }
        public string CompanyName { get; set; }
        public string ApplicantName { get; set; }
        public DateTime ApplicationDate { get; set; }
    }
}
