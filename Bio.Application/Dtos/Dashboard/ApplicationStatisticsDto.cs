namespace Bio.Application.Dtos.Dashboard
{
    public class ApplicationStatisticsDto
    {
        public int TotalApplications { get; set; }
        public List<RecentApplicationDto> RecentApplications { get; set; }
        public List<MostAppliedCompaniesDto> MostAppliedCompanies { get; set; }
    }
}
