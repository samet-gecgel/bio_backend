namespace Bio.Application.Dtos.Dashboard
{
    public class PlatformStatisticsDto
    {
        public UserStatisticsDto UserStatistics { get; set; }
        public CompanyStatisticsDto CompanyStatistics { get; set; }
        public ApplicationStatisticsDto ApplicationStatistics { get; set; }
        public JobPostStatisticsDto JobPostStatistics { get; set; }
    }

}
