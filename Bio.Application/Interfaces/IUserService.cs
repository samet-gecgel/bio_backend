using Bio.Application.Dtos.User;
using Bio.Domain.Enums;
using BlogProject.Application.Result;

namespace Bio.Application.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResult<UserDto>> GetUserByIdAsync(Guid userId);
        Task<ServiceResult<IEnumerable<UserDto>>> GetAllUsersAsync();
        Task<ServiceResult<IEnumerable<UserDto>>> GetUsersByApprovalStatusAsync(AccountApprovalStatus status);
        Task<ServiceResult<string>> LoginAsync(UserLoginDto userLoginDto);
        Task<ServiceResult<IEnumerable<UserDto>>> GetUsersPagedAsync(int pageNumber, int pageSize);
        Task<ServiceResult<UserDto>> CreateUserAsync(UserCreateDto userCreateDto);
        Task<ServiceResult> UpdateUserAsync(Guid id, UserUpdateDto userUpdateDto);
        Task<ServiceResult> UpdateUserPasswordAsync(Guid id, UserPasswordUpdateDto passwordUpdateDto);
        Task<ServiceResult> DeleteUserAsync(Guid id);
        Task<ServiceResult> ApproveUserAsync(Guid id);
        Task<ServiceResult> RejectUserAsync(Guid id);
    }
}
