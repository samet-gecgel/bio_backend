using Bio.Application.Dtos.JobPost;
using Bio.Domain.Entities;
using Bio.Domain.Enums;
using BlogProject.Application.Result;

namespace Bio.Application.Interfaces
{
    public interface IJobPostService
    {
        Task<ServiceResult<JobPostDto>> GetJobPostByIdAsync(Guid id);
        Task<ServiceResult<IEnumerable<JobPostDto>>> GetAllJobPostsAsync();
        Task<ServiceResult<IEnumerable<JobPostDto>>> GetActiveJobPostAsync();
        Task<ServiceResult<IEnumerable<JobPostDto>>> GetJobPostsByCompanyIdPagedAsync(int pageNumber, int pageSize);
        Task<ServiceResult<IEnumerable<JobPostDto>>> GetJobPostsByCategoryIdAsync(Guid categoryId);
        Task<ServiceResult<IEnumerable<JobPostDto>>> GetJobPostsByCompanyIdAsync();
        Task<ServiceResult<IEnumerable<JobPostDto>>> GetJobPostsPagedAsync(int pageNumber, int pageSize);
        Task<ServiceResult<IEnumerable<JobPostDto>>> GetPagedJobPostsAsync(int pageNumber, int pageSize, JobPostFilterDto filter);
        Task<ServiceResult<IEnumerable<JobPostDto>>> GetLatestJobPostsAsync(int count = 4);
        Task<ServiceResult> ApproveJobPostAsync(Guid id);
        Task<ServiceResult> RejectJobPostAsync(Guid id);
        Task<ServiceResult<JobPostDto>> CreateJobPostAsync(JobPostCreateDto jobPostCreateDto);
        Task<ServiceResult> UpdateJobPostAsync(Guid id, JobPostUpdateDto jobPostUpdateDto);
        Task<ServiceResult> DeleteJobPostAsync(Guid id);

    }
}
