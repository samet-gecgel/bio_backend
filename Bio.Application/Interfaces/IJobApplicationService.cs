using Bio.Application.Dtos.JobApplication;
using Bio.Domain.Entities;
using BlogProject.Application.Result;

namespace Bio.Application.Interfaces
{
    public interface IJobApplicationService
    {
        Task<ServiceResult<JobApplicationDto>> GetJobApplicationByIdAsync(Guid id);
        Task<ServiceResult<IEnumerable<JobApplicationDto>>> GetAllJobApplicationsAsync();
        Task<ServiceResult<IEnumerable<JobApplicationDto>>> GetJobApplicationsByUserIdAsync();
        Task<ServiceResult<IEnumerable<JobApplicationDto>>> GetJobApplicationsByJobPostIdAsync(Guid jobPostId);
        Task<ServiceResult<JobApplicationDto>> CreateJobApplicationAsync(JobApplicationCreateDto jobApplicationCreateDto);
        Task<ServiceResult<IEnumerable<JobApplicationDto>>> GetApplicationsByCompanyIdAsync(Guid companyId);
        Task<ServiceResult<JobApplicationDetailDto>> GetApplicationDetailByIdAsync(Guid applicationId);
        Task<ServiceResult> UpdateJobApplicationAsync(Guid id, JobApplicationUpdateDto jobApplicationUpdateDto);
        Task<ServiceResult> DeleteJobApplicationAsync(Guid id);
        Task<ServiceResult> ApproveJobApplicationAsync(Guid id);
        Task<ServiceResult> RejectJobApplicationAsync(Guid id);
    }
}
