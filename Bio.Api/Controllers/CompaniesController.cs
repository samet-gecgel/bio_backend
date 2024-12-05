using Bio.Application.Dtos.Admin;
using Bio.Application.Dtos.Company;
using Bio.Application.Interfaces;
using Bio.Application.Services;
using Bio.Domain.Enums;
using BlogProject.Application.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Bio.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : CustomBaseController
    {
        private readonly ICompanyService _companyService;

        public CompaniesController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> GetAllCompanies()
        {
            var result = await _companyService.GetAllCompaniesAsync();
            return CreateActionResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompany([FromBody] CompanyCreateDto companyCreateDto)
        {
            var result = await _companyService.CreateCompanyAsync(companyCreateDto);
            return CreateActionResult(result);
        }


        [HttpGet("approval-status/{status}")]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> GetCompaniesByApprovalStatus(AccountApprovalStatus status)
        {
            var result = await _companyService.GetCompaniesByApprovalStatusAsync(status);
            return CreateActionResult(result);
        }

        [HttpGet("public-companies")]
        [AllowAnonymous]
        public async Task<IActionResult> getPublicCompanies()
        {
            var result = await _companyService.GetPublicCompaniesAsync();
            return CreateActionResult(result);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(CompanyLoginDto companyLoginDto)
        {
            var result = await _companyService.LoginAsync(companyLoginDto);
            return CreateActionResult(result);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Policy = "RequireCompanyOrAdminOrSuperAdmin")]
        public async Task<IActionResult> GetCompanyById(Guid id)
        {
            var result = await _companyService.GetCompanyByIdAsync(id);
            return CreateActionResult(result);
        }

        [HttpGet("paged")]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> GetCompaniesPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _companyService.GetCompaniesPagedAsync(pageNumber, pageSize);
            return CreateActionResult(result);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Policy = "RequireCompany")]
        public async Task<IActionResult> UpdateCompany(Guid id, [FromBody] CompanyUpdateDto companyUpdateDto)
        {
            var result = await _companyService.UpdateCompanyAsync(id, companyUpdateDto);
            return CreateActionResult(result);
        }

        [HttpPost("{id:guid}/approve")]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> ApproveCompany(Guid id)
        {
            var result = await _companyService.ApproveCompanyAsync(id);
            return CreateActionResult(result);
        }

        [HttpPost("{id:guid}/reject")]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> RejectCompany(Guid id)
        {
            var result = await _companyService.RejectCompanyAsync(id);
            return CreateActionResult(result);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Policy = "RequireCompanyOrAdminOrSuperAdmin")]
        public async Task<IActionResult> DeleteCompany(Guid id)
        {
            var result = await _companyService.DeleteCompanyAsync(id);
            return CreateActionResult(result);
        }

        [HttpPut("{id:guid}/update-password")]
        [Authorize(Policy = "RequireCompany")]
        public async Task<IActionResult> UpdateCompanyPassword(Guid id, [FromBody] CompanyPasswordUpdateDto passwordUpdateDto)
        {
            var result = await _companyService.UpdateCompanyPasswordAsync(id, passwordUpdateDto);
            return CreateActionResult(result);
        }
    }
}
