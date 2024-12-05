namespace Bio.Application.Dtos.Dashboard
{
    public class JobPostStatisticsDto
    {
        public int TotalJobPosts { get; set; }
        public int ActiveJobPosts { get; set; }
        public int NewJobPostsLast30Days { get; set; }
    }

}
