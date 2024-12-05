using Bio.Application.Dtos.Admin;
using BlogProject.Application.Result;

namespace Bio.Application.Interfaces
{
    public interface IAdminService
    {
        Task<ServiceResult<AdminDto>> GetAdminByIdAsync(Guid id);
        Task<ServiceResult<IEnumerable<AdminDto>>> GetAllAdminAsync();
        Task<ServiceResult<string>> LoginAsync(AdminLoginDto adminLoginDto);

        Task<ServiceResult<IEnumerable<AdminDto>>> GetAdminsPagedAsync(int pageNumber, int pageSize);

        Task<ServiceResult> UpdateAdminPasswordAsync(Guid id, AdminPasswordUpdateDto passwordUpdateDto);
        Task<ServiceResult<AdminDto>> CreateAdminAsync(AdminCreateDto adminCreateDto);
        Task<ServiceResult> UpdateAdminAsync(Guid id, AdminUpdateDto adminUpdateDto);
        Task<ServiceResult> DeleteAdminAsync(Guid id);
    }
}
