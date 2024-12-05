using Bio.Domain.Entities;
using Bio.Domain.Enums;

namespace Bio.Domain.Repositories
{
    public interface ICompanyRepository : IBaseRepository<Company>
    {
        Task<Company> GetVknAsync(string vkn);
        Task<Company> GetByEmailAsync(string email);
        Task<IEnumerable<Company>> GetCompaniesByApprovalStatusAsync(AccountApprovalStatus status); 
        Task<Company> GetCompanyWithJobPosts(Guid companyId);
        Task<(IEnumerable<Company>, int)> GetAllCompanyPagedAsync(int pageNumber, int pageSize);
    }
}
