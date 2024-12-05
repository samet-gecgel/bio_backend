using Bio.Domain.Entities;
using Bio.Domain.Enums;

namespace Bio.Domain.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> GetEmailAsync(string email);
        Task<IEnumerable<User>> GetUsersByApprovalStatusAsync(AccountApprovalStatus status);
        Task<User> GetUserWithResumesCertificatesAndJobApplication(Guid userId);
        Task<(IEnumerable<User>, int)> GetAllUserPagedAsync(int pageNumber, int pageSize);
    }
}
