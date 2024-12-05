using Bio.Application.Dtos.Dashboard;
using BlogProject.Application.Result;

namespace Bio.Application.Interfaces
{
    public interface IDashboardCompanyService
    {
        Task<ServiceResult<CompanyDashboardDto>> GetCompanyDashboardAsync();
    }
}
