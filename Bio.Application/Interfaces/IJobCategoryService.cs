using Bio.Application.Dtos.JobCategory;
using Bio.Domain.Entities;
using BlogProject.Application.Result;

namespace Bio.Application.Interfaces
{
    public interface IJobCategoryService
    {
        Task<ServiceResult<JobCategoryDto>> GetJobCategoryByIdAsync(Guid id);
        Task<ServiceResult<IEnumerable<JobCategoryDto>>> GetAllJobCategoriesAsync();
        Task<ServiceResult<JobCategoryDto>> CreateJobCategoryAsync(JobCategoryCreateDto jobCategoryCreateDto);
        Task<ServiceResult> UpdateJobCategoryAsync(Guid id, JobCategoryUpdateDto jobCategoryUpdateDto);
        Task<ServiceResult> DeleteJobCategoryAsync(Guid id);
    }
}
