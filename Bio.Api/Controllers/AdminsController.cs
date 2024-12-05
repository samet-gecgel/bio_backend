using Bio.Application.Dtos.Admin;
using Bio.Application.Dtos.User;
using Bio.Application.Interfaces;
using Bio.Application.Services;
using BlogProject.Application.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bio.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AdminsController : CustomBaseController
    {
        private readonly IAdminService _adminService;

        public AdminsController(IAdminService adminService)
        {
            _adminService = adminService;
        }


        [HttpPost("create")]
        [Authorize(Policy = "RequireSuperAdmin")]
        public async Task<IActionResult> CreateAdmin(AdminCreateDto adminCreateDto)
        {
            var result = await _adminService.CreateAdminAsync(adminCreateDto);
            return CreateActionResult(result);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> GetAdminById(Guid id)
        {
            var result = await _adminService.GetAdminByIdAsync(id);
            return CreateActionResult(result);
        }

        [HttpGet("all")]
        [Authorize(Policy = "RequireSuperAdmin")]
        public async Task<IActionResult> GetAllAdmins()
        {
            var result = await _adminService.GetAllAdminAsync();
            return CreateActionResult(result);
        }

        [HttpPut("update/{id}")]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> UpdateAdmin(Guid id, AdminUpdateDto adminUpdateDto)
        {
            var result = await _adminService.UpdateAdminAsync(id, adminUpdateDto);
            return CreateActionResult(result);
        }

        [HttpPut("{id:guid}/update-password")]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> UpdateAdminPassword(Guid id, [FromBody] AdminPasswordUpdateDto passwordUpdateDto)
        {
            var result = await _adminService.UpdateAdminPasswordAsync(id, passwordUpdateDto);
            return CreateActionResult(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "RequireSuperAdmin")]
        public async Task<IActionResult> DeleteAdmin(Guid id)
        {
            var result = await _adminService.DeleteAdminAsync(id);
            return CreateActionResult(result);
        }

        [HttpGet("paged")]
        [Authorize(Policy = "RequireSuperAdmin")]
        public async Task<IActionResult> GetAdminsPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _adminService.GetAdminsPagedAsync(pageNumber, pageSize);
            return CreateActionResult(result);
        }


        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(AdminLoginDto adminLoginDto)
        {
            var result = await _adminService.LoginAsync(adminLoginDto);
            return CreateActionResult(result);
        }
    }
}
