namespace Bio.Application.Dtos.Dashboard
{
    public class UserStatisticsDto
    {
        public int TotalUsers { get; set; }
        public int ApprovedUsers { get; set; }
        public int NewUsersLast30Days { get; set; }
    }
}
