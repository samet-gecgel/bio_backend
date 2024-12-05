using Bio.Application.Dtos.JobPost;
using Bio.Application.Interfaces;
using Bio.Application.Services;
using Bio.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bio.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobPostsController : CustomBaseController
    {
        private readonly IJobPostService _jobPostService;

        public JobPostsController(IJobPostService jobPostService)
        {
            _jobPostService = jobPostService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllJobPosts()
        {
            var result = await _jobPostService.GetAllJobPostsAsync();
            return CreateActionResult(result);
        }


        [HttpGet("active")]
        public async Task<IActionResult> GetActiveJobPosts()
        {
            var result = await _jobPostService.GetActiveJobPostAsync();
            return CreateActionResult(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetJobPostById(Guid id)
        {
            var result = await _jobPostService.GetJobPostByIdAsync(id);
            return CreateActionResult(result);
        }

        [HttpGet("category/{categoryId:guid}")]
        public async Task<IActionResult> GetJobPostsByCategoryId(Guid categoryId)
        {
            var result = await _jobPostService.GetJobPostsByCategoryIdAsync(categoryId);
            return CreateActionResult(result);
        }

        [HttpGet("company")]
        [Authorize(Policy = "RequireCompany")]
        public async Task<IActionResult> GetJobPostsByCompanyId()
        {
            var result = await _jobPostService.GetJobPostsByCompanyIdAsync();
            return CreateActionResult(result);
        }

        [HttpGet("company/paged")]
        [Authorize(Policy = "RequireCompany")]
        public async Task<IActionResult> GetJobPostsByCompanyPagedId([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _jobPostService.GetJobPostsByCompanyIdPagedAsync(pageNumber, pageSize);
            return CreateActionResult(result);
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetJobPostsPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _jobPostService.GetJobPostsPagedAsync(pageNumber, pageSize);
            return CreateActionResult(result);
        }

        [HttpGet("GetPagedJobPosts")]
        public async Task<IActionResult> GetPagedJobPosts([FromQuery] int pageNumber = 1,[FromQuery] int pageSize = 10, [FromQuery] JobPostFilterDto? filter = null)
        {
            var result = await _jobPostService.GetPagedJobPostsAsync(pageNumber, pageSize, filter);
            return CreateActionResult(result);
        }

        [HttpPost("{id:guid}/approve")]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> ApproveJobPost(Guid id)
        {
            var result = await _jobPostService.ApproveJobPostAsync(id);
            return CreateActionResult(result);
        }

        [HttpPost("{id:guid}/reject")]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> RejectJobPost(Guid id)
        {
            var result = await _jobPostService.RejectJobPostAsync(id);
            return CreateActionResult(result);
        }

        [HttpPost]
        [Authorize(Policy = "RequireCompany")]
        public async Task<IActionResult> CreateJobPost(JobPostCreateDto jobPostCreateDto)
        {
            var result = await _jobPostService.CreateJobPostAsync(jobPostCreateDto);
            return CreateActionResult(result);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Policy = "RequireCompany")]
        public async Task<IActionResult> UpdateJobPost(Guid id, JobPostUpdateDto jobPostUpdateDto)
        {
            var result = await _jobPostService.UpdateJobPostAsync(id, jobPostUpdateDto);
            return CreateActionResult(result);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> DeleteJobPost(Guid id)
        {
            var result = await _jobPostService.DeleteJobPostAsync(id);
            return CreateActionResult(result);
        }

        [HttpGet("latest")]
        public async Task<IActionResult> GetLatestJobPosts([FromQuery] int count = 4)
        {
            var result = await _jobPostService.GetLatestJobPostsAsync(count);
            return CreateActionResult(result);
        }

    }
}
