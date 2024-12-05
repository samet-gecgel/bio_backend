using Bio.Application.Dtos.Certificate;
using BlogProject.Application.Result;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bio.Application.Interfaces
{
    public interface ICertificateService
    {
        Task<ServiceResult<CertificateDto>> GetCertificateByIdAsync(Guid id);
        Task<ServiceResult<IEnumerable<CertificateDto>>> GetCertificatesByUserIdAsync();
        Task<ServiceResult<CertificateDto>> CreateCertificateAsync(CertificateCreateDto certificateCreateDto, IFormFile filePath);
        Task<ServiceResult> UpdateCertificateAsync(Guid id, CertificateUpdateDto certificateUpdateDto, IFormFile filePath);
        Task<ServiceResult> DeleteCertificateAsync(Guid id);
    }
}
