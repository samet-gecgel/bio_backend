using Bio.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bio.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "RequireCompany")]
    public class DashboardCompaniesController : CustomBaseController
    {
        private readonly IDashboardCompanyService _dashboardCompanyService;

        public DashboardCompaniesController(IDashboardCompanyService dashboardCompanyService)
        {
            _dashboardCompanyService = dashboardCompanyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCompanyDashboard()
        {
            var result = await _dashboardCompanyService.GetCompanyDashboardAsync();
            return CreateActionResult(result);
        }
    }
}
