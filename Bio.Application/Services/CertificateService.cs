using AutoMapper;
using Bio.Application.Dtos.Certificate;
using Bio.Application.Interfaces;
using Bio.Domain.Entities;
using Bio.Domain.Interfaces;
using Bio.Domain.Repositories;
using BlogProject.Application.Result;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Security.Claims;

namespace Bio.Application.Services
{
    public class CertificateService : ICertificateService
    {
        private readonly ICertificateRepository _certificateRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFileService _fileService;
        private readonly IUnitOfWork _unitOfWork;

        public CertificateService(ICertificateRepository certificateRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor, IFileService fileService , IUnitOfWork unitOfWork)
        {
            _certificateRepository = certificateRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _fileService = fileService;
            _unitOfWork = unitOfWork;
        }
        private Guid GetUserIdFromToken()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                throw new UnauthorizedAccessException("Kullanıcı kimliği doğrulanamadı veya geçersiz.");
            }

            return userId;
        }



        public async Task<ServiceResult<CertificateDto>> CreateCertificateAsync(CertificateCreateDto certificateCreateDto, IFormFile filePath)
        {
            var userId = GetUserIdFromToken();

            string file = null;
            if (filePath != null)
            {
                file = await _fileService.SaveFileAsync(filePath, "Certificates", Guid.NewGuid().ToString());
            }

            var certificate = _mapper.Map<Certificate>(certificateCreateDto);
            certificate.UserId = userId;
            certificate.FilePath = file;

            await _certificateRepository.AddAsync(certificate);

            var certificateDto = _mapper.Map<CertificateDto>(certificate);

            await _unitOfWork.SaveChangesAsync();

            return ServiceResult<CertificateDto>.SuccessAsCreated(certificateDto);
        }



        public async Task<ServiceResult> DeleteCertificateAsync(Guid id)
        {
            var userId = GetUserIdFromToken();

            var certificate = await _certificateRepository.GetByIdAsync(id);
            if (certificate == null)
            {
                return ServiceResult.Fail("Sertifika bulunamadı veya silme yetkiniz yok", HttpStatusCode.NotFound);
            }

            _certificateRepository.Delete(certificate);

            await _unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult<CertificateDto>> GetCertificateByIdAsync(Guid id)
        {
            var certificate = await _certificateRepository.GetByIdAsync(id);

            if(certificate == null)
            {
                return ServiceResult<CertificateDto>.Fail("Sertifika bulunamadı", HttpStatusCode.NotFound);
            }

            var certificateDto = _mapper.Map<CertificateDto>(certificate);
            return ServiceResult<CertificateDto>.Success(certificateDto);
        }

        public async Task<ServiceResult<IEnumerable<CertificateDto>>> GetCertificatesByUserIdAsync()
        {
            var userId = GetUserIdFromToken();

            var certificates = await _certificateRepository.GetByUserIdAsync(userId);

            if(certificates == null || !certificates.Any())
            {
                return ServiceResult<IEnumerable<CertificateDto>>.Fail("Kullanıcıya ait sertifika bulunamadı", HttpStatusCode.NotFound);
            }

            var certificateDtos = _mapper.Map<IEnumerable<CertificateDto>>(certificates);
            return ServiceResult<IEnumerable<CertificateDto>>.Success(certificateDtos);
        }

        public async Task<ServiceResult> UpdateCertificateAsync(Guid id, CertificateUpdateDto certificateUpdateDto, IFormFile file)
        {
            var certificate = await _certificateRepository.GetByIdAsync(id);

            if (certificate == null)
            {
                return ServiceResult.Fail("Sertifika bulunamadı", HttpStatusCode.NotFound);
            }

            if (file != null)
            {
                if (!string.IsNullOrEmpty(certificate.FilePath))
                {
                    _fileService.DeleteFile(certificate.FilePath);
                }

                var newFilePath = await _fileService.SaveFileAsync(file, "Certificates", Guid.NewGuid().ToString());
                certificate.FilePath = newFilePath;
            }

            _mapper.Map(certificateUpdateDto, certificate);
            _certificateRepository.Update(certificate);

            await _unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }
    }
}
