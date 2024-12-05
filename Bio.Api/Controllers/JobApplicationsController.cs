using Bio.Application.Dtos.JobApplication;
using Bio.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bio.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobApplicationsController : CustomBaseController
    {
        private readonly IJobApplicationService _jobApplicationService;

        public JobApplicationsController(IJobApplicationService jobApplicationService)
        {
            _jobApplicationService = jobApplicationService;
        }

        [HttpGet("user")]
        [Authorize(Policy = "RequireJobSeeker")]
        public async Task<IActionResult> GetJobApplicationsByUserId()
        {
            var result = await _jobApplicationService.GetJobApplicationsByUserIdAsync();
            return CreateActionResult(result);
        }

        [HttpGet("jobPost/{jobPostId:guid}")]
        [Authorize(Policy = "RequireCompany")]
        public async Task<IActionResult> GetJobApplicationsByJobPostId(Guid jobPostId)
        {
            var result = await _jobApplicationService.GetJobApplicationsByJobPostIdAsync(jobPostId);
            return CreateActionResult(result);
        }

        [HttpGet]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> GetAllJobApplications()
        {
            var result = await _jobApplicationService.GetAllJobApplicationsAsync();
            return CreateActionResult(result);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Policy = "RequireCompanyOrAJobSeekerOrAdmin")]
        public async Task<IActionResult> GetJobApplicationById(Guid id)
        {
            var result = await _jobApplicationService.GetJobApplicationByIdAsync(id);
            return CreateActionResult(result);
        }

        [HttpGet("company/{companyId}/applications")]
        [Authorize(Policy = "RequireCompany")]
        public async Task<IActionResult> GetApplicationsByCompanyId(Guid companyId)
        {
            var result = await _jobApplicationService.GetApplicationsByCompanyIdAsync(companyId);
            return CreateActionResult(result);
        }

        [HttpGet("applications/{applicationId}/detail")]
        [Authorize(Policy = "RequireCompany")]
        public async Task<IActionResult> GetApplicationDetail(Guid applicationId)
        {
            var result = await _jobApplicationService.GetApplicationDetailByIdAsync(applicationId);
            return CreateActionResult(result);
        }



        [HttpPost]
        [Authorize(Policy = "RequireJobSeeker")]
        public async Task<IActionResult> CreateJobApplication(JobApplicationCreateDto jobApplicationCreateDto)
        {
            var result = await _jobApplicationService.CreateJobApplicationAsync(jobApplicationCreateDto);
            return CreateActionResult(result);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Policy = "RequireJobSeeker")]
        public async Task<IActionResult> UpdateJobApplication(Guid id, JobApplicationUpdateDto jobApplicationUpdateDto)
        {
            var result = await _jobApplicationService.UpdateJobApplicationAsync(id, jobApplicationUpdateDto);
            return CreateActionResult(result);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Policy = "RequireJobSeeker")]
        public async Task<IActionResult> DeleteJobApplication(Guid id)
        {
            var result = await _jobApplicationService.DeleteJobApplicationAsync(id);
            return CreateActionResult(result);
        }

        [HttpPost("{id:guid}/approve")]
        [Authorize(Policy = "RequireCompany")]
        public async Task<IActionResult> ApproveJobApplication(Guid id)
        {
            var result = await _jobApplicationService.ApproveJobApplicationAsync(id);
            return CreateActionResult(result);
        }

        [HttpPost("{id:guid}/reject")]
        [Authorize(Policy = "RequireCompany")]
        public async Task<IActionResult> RejectJobApplication(Guid id)
        {
            var result = await _jobApplicationService.RejectJobApplicationAsync(id);
            return CreateActionResult(result);
        }
    }
}
