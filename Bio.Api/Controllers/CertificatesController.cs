using Bio.Application.Dtos.Certificate;
using Bio.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bio.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "RequireJobSeeker")]
    public class CertificatesController : CustomBaseController
    {
        private readonly ICertificateService _certificateService;

        public CertificatesController(ICertificateService certificateService)
        {
            _certificateService = certificateService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCertificate([FromForm] CertificateCreateDto certificateCreateDto, IFormFile filePath)
        {
            var result = await _certificateService.CreateCertificateAsync(certificateCreateDto, filePath);
            return CreateActionResult(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCertificate(Guid id, [FromForm] CertificateUpdateDto certificateUpdateDto, IFormFile? filePath)
        {
            var result = await _certificateService.UpdateCertificateAsync(id, certificateUpdateDto, filePath);
            return CreateActionResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCertificate(Guid id)
        {
            var result = await _certificateService.DeleteCertificateAsync(id);
            return CreateActionResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCertificateById(Guid id)
        {
            var result = await _certificateService.GetCertificateByIdAsync(id);
            return CreateActionResult(result);
        }

        [HttpGet("user-certificates")]
        public async Task<IActionResult> GetCertificatesByUserId()
        {
            var result = await _certificateService.GetCertificatesByUserIdAsync();
            return CreateActionResult(result);
        }
    }
}
