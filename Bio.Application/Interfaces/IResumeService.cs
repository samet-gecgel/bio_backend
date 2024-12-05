using Bio.Application.Dtos.Resume;
using BlogProject.Application.Result;

namespace Bio.Application.Interfaces
{
    public interface IResumeService
    {
        Task<ServiceResult<ResumeDto>> GetResumeByIdAsync(Guid id);
        Task<ServiceResult<IEnumerable<ResumeDto>>> GetAllResumesAsync();
        Task<ServiceResult<IEnumerable<ResumeDto>>> GetResumesByUserIdAsync(Guid userId);
        Task<ServiceResult<ResumeDto>> GetResumeByJobApplicationIdAsync(Guid jobApplicationId);
        Task<ServiceResult<ResumeDto>> CreateResumeAsync(ResumeCreateDto resumeCreateDto);
        Task<ServiceResult> UpdateResumeAsync(Guid id, ResumeUpdateDto resumeUpdateDto);
        Task<ServiceResult> DeleteResumeAsync(Guid id);
    }
}
