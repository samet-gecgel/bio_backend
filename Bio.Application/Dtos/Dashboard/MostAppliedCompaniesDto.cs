namespace Bio.Application.Dtos.Dashboard
{
    public class MostAppliedCompaniesDto
    {
        public Guid Id { get; set; }
        public string CompanyName { get; set; }
        public int TotalApplications { get; set; }
    }
}
