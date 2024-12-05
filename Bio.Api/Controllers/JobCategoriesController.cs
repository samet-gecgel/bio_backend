using Bio.Application.Dtos.JobCategory;
using Bio.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bio.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobCategoriesController : CustomBaseController
    {
        private readonly IJobCategoryService _jobCategoryService;

        public JobCategoriesController(IJobCategoryService jobCategoryService)
        {
            _jobCategoryService = jobCategoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllJobCategories()
        {
            var result = await _jobCategoryService.GetAllJobCategoriesAsync();
            return CreateActionResult(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetJobCategoryById(Guid id)
        {
            var result = await _jobCategoryService.GetJobCategoryByIdAsync(id);
            return CreateActionResult(result);
        }

        [Authorize(Policy = "RequireAdmin")]
        [HttpPost]
        public async Task<IActionResult> CreateJobCategory(JobCategoryCreateDto jobCategoryCreateDto)
        {
            var result = await _jobCategoryService.CreateJobCategoryAsync(jobCategoryCreateDto);
            return CreateActionResult(result);
        }

        [Authorize(Policy = "RequireAdmin")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateJobCategory(Guid id, JobCategoryUpdateDto jobCategoryUpdateDto)
        {
            var result = await _jobCategoryService.UpdateJobCategoryAsync(id, jobCategoryUpdateDto);
            return CreateActionResult(result);
        }

        [Authorize(Policy = "RequireAdmin")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteJobCategory(Guid id)
        {
            var result = await _jobCategoryService.DeleteJobCategoryAsync(id);
            return CreateActionResult(result);
        }
    }
}
