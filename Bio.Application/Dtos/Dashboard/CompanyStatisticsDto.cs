namespace Bio.Application.Dtos.Dashboard
{
    public class CompanyStatisticsDto
    {
        public int TotalCompanies { get; set; }
        public int ApprovedCompanies { get; set; }
        public int NewCompaniesLast30Days { get; set; }
    }
}
