using Bio.Application.Dtos.Resume;
using Bio.Application.Interfaces;
using Bio.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bio.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResumesController : CustomBaseController
    {
        private readonly IResumeService _resumeService;

        public ResumesController(IResumeService resumeService)
        {
            _resumeService = resumeService;
        }

        [HttpGet]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> GetAllResumes()
        {
            var result = await _resumeService.GetAllResumesAsync();
            return CreateActionResult(result);
        }

        [HttpGet("user/{userId:guid}")]
        [Authorize(Policy = "RequireJobSeekerOrAdmin")]
        public async Task<IActionResult> GetResumesByUserId(Guid userId)
        {
            var result = await _resumeService.GetResumesByUserIdAsync(userId);
            return CreateActionResult(result);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Policy = "RequireCompanyOrAJobSeekerOrAdmin")]
        public async Task<IActionResult> GetResumeById(Guid id)
        {
            var result = await _resumeService.GetResumeByIdAsync(id);
            return CreateActionResult(result);
        }

        [HttpGet("jobApplication/{jobApplicationId:guid}/resume")]
        [Authorize(Policy = "RequireCompanyOrAJobSeekerOrAdmin")]
        public async Task<IActionResult> GetResumeByJobApplicationId(Guid jobApplicationId)
        {
            var result = await _resumeService.GetResumeByJobApplicationIdAsync(jobApplicationId);
            return CreateActionResult(result);
        }

        [HttpPost]
        [Authorize(Policy = "RequireJobSeeker")]
        public async Task<IActionResult> CreateResume(ResumeCreateDto resumeCreateDto)
        {
            var result = await _resumeService.CreateResumeAsync(resumeCreateDto);
            return CreateActionResult(result);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Policy = "RequireJobSeeker")]
        public async Task<IActionResult> UpdateResume(Guid id, ResumeUpdateDto resumeUpdateDto)
        {
            var result = await _resumeService.UpdateResumeAsync(id, resumeUpdateDto);
            return CreateActionResult(result);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Policy = "RequireJobSeeker")]
        public async Task<IActionResult> DeleteResume(Guid id)
        {
            var result = await _resumeService.DeleteResumeAsync(id);
            return CreateActionResult(result);
        }
    }
}
