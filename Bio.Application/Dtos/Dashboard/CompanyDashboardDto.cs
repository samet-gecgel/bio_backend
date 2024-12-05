using Bio.Application.Dtos.JobPost;

namespace Bio.Application.Dtos.Dashboard
{
    public class CompanyDashboardDto
    {
        public int TotalJobPosts { get; set; }
        public int ActiveJobPosts { get; set; }
        public int ApprovedJobPosts { get; set; }
        public int PendingJobPosts { get; set; }
        public int RejectedJobPosts { get; set; }
        public int TotalApplications { get; set; }
        public int TotalJobPostViews { get; set; }
        public List<JobPostDto> ActiveJobPostsTable { get; set; }
        public List<RecentApplicationDto> RecentApplications { get; set; }
        public List<KeyValuePair<string, int>> JobApplicationsLast7Days { get; set; }
        public List<KeyValuePair<string, int>> WeeklyJobPostViews { get; set; }
    }
}
