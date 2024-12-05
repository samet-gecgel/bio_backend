using Bio.Application.Dtos.Company;
using Bio.Application.Dtos.User;
using Bio.Application.Interfaces;
using Bio.Application.Services;
using Bio.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bio.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : CustomBaseController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserCreateDto userCreateDto)
        {
            var result = await _userService.CreateUserAsync(userCreateDto);
            return CreateActionResult(result);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {
            var result = await _userService.LoginAsync(userLoginDto);
            return CreateActionResult(result);
        }

        [HttpGet]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _userService.GetAllUsersAsync();
            return CreateActionResult(result);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Policy = "RequireJobSeekerOrAdmin")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var result = await _userService.GetUserByIdAsync(id);
            return CreateActionResult(result);
        }

        [HttpGet("paged")]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> GetUsersPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _userService.GetUsersPagedAsync(pageNumber, pageSize);
            return CreateActionResult(result);
        }


        [HttpGet("approval-status/{status}")]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> GetUsersByApprovalStatus(AccountApprovalStatus status)
        {
            var result = await _userService.GetUsersByApprovalStatusAsync(status);
            return CreateActionResult(result);
        }

        [HttpPost("{id:guid}/approve")]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> ApproveUser(Guid id)
        {
            var result = await _userService.ApproveUserAsync(id);
            return CreateActionResult(result);
        }

        [HttpPost("{id:guid}/reject")]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> RejectUser(Guid id)
        {
            var result = await _userService.RejectUserAsync(id);
            return CreateActionResult(result);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var result = await _userService.DeleteUserAsync(id);
            return CreateActionResult(result);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Policy = "RequireJobSeeker")]
        public async Task<IActionResult> UpdateUser(Guid id, UserUpdateDto userUpdateDto)
        {
            var result = await _userService.UpdateUserAsync(id, userUpdateDto);
            return CreateActionResult(result);
        }

        [HttpPut("{id:guid}/update-password")]
        [Authorize(Policy = "RequireJobSeeker")]
        public async Task<IActionResult> UpdateUserPassword(Guid id, [FromBody] UserPasswordUpdateDto passwordUpdateDto)
        {
            var result = await _userService.UpdateUserPasswordAsync(id, passwordUpdateDto);
            return CreateActionResult(result);
        }
    }
}
