using Bio.Application.Dtos.Company;
using Bio.Application.Dtos.User;
using Bio.Domain.Enums;
using BlogProject.Application.Result;
using Microsoft.AspNetCore.Http;

namespace Bio.Application.Interfaces
{
    public interface ICompanyService
    {
        Task<ServiceResult<CompanyDto>> GetCompanyByIdAsync(Guid id);
        Task<ServiceResult<IEnumerable<CompanyDto>>> GetAllCompaniesAsync();
        Task<ServiceResult<IEnumerable<CompanyDto>>> GetCompaniesByApprovalStatusAsync(AccountApprovalStatus approvalStatus);
        Task<ServiceResult<IEnumerable<PublicCompanyDto>>> GetPublicCompaniesAsync();
        Task<ServiceResult<string>> LoginAsync(CompanyLoginDto companyLoginDto);
        Task<ServiceResult<IEnumerable<CompanyDto>>> GetCompaniesPagedAsync(int pageNumber, int pageSize);
        Task<ServiceResult<CompanyDto>> CreateCompanyAsync(CompanyCreateDto companyCreateDto);
        Task<ServiceResult> UpdateCompanyAsync(Guid id, CompanyUpdateDto companyUpdateDto);
        Task<ServiceResult> UpdateCompanyPasswordAsync(Guid id, CompanyPasswordUpdateDto passwordUpdateDto);
        Task<ServiceResult> DeleteCompanyAsync(Guid id);
        Task<ServiceResult> ApproveCompanyAsync(Guid id);
        Task<ServiceResult> RejectCompanyAsync(Guid id);

    }
}
