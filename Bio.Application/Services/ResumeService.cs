using AutoMapper;
using Bio.Application.Dtos.Resume;
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
    public class ResumeService : IResumeService
    {
        private readonly IResumeRepository _resumeRepository;
        private readonly IJobApplicationRepository _jobApplicationRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;

        public ResumeService(IResumeRepository resumeRepository, IJobApplicationRepository jobApplicationRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork)
        {
            _resumeRepository = resumeRepository;
            _jobApplicationRepository = jobApplicationRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
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

        public async Task<ServiceResult<ResumeDto>> CreateResumeAsync(ResumeCreateDto resumeCreateDto)
        {
            var userId = GetUserIdFromToken();
            var resume = _mapper.Map<Resume>(resumeCreateDto);
            resume.UserId = userId;
            resume.CreatedAt = DateTime.Now;
            
            await _resumeRepository.AddAsync(resume);

            var resumeDto = _mapper.Map<ResumeDto>(resume);

            await _unitOfWork.SaveChangesAsync();

            return ServiceResult<ResumeDto>.SuccessAsCreated(resumeDto);
        }

        public async Task<ServiceResult> DeleteResumeAsync(Guid id)
        {
            var resume = await _resumeRepository.GetByIdAsync(id);
            
            if(resume == null)
            {
                return ServiceResult.Fail("Özgeçmiş bulunamadı.");
            }

            _resumeRepository.Delete(resume);

            await _unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult<IEnumerable<ResumeDto>>> GetAllResumesAsync()
        {
            var resumes = await _resumeRepository.GetAllAsync();
            if(resumes == null || !resumes.Any())
            {
                return ServiceResult<IEnumerable<ResumeDto>>.Fail("Henüz bir özgeçmiş bulunmamaktadır.");
            }

            var resumeDtos = _mapper.Map<IEnumerable<ResumeDto>>(resumes);

            return ServiceResult<IEnumerable<ResumeDto>>.Success(resumeDtos);
        }

        public async Task<ServiceResult<ResumeDto>> GetResumeByIdAsync(Guid id)
        {
            var resume = await _resumeRepository.GetByIdAsync(id);
            if (resume == null)
            {
                return ServiceResult<ResumeDto>.Fail("Özgeçmiş bulunamadı.", HttpStatusCode.NotFound);
            }

            var resumeDto = _mapper.Map<ResumeDto>(resume);
            return ServiceResult<ResumeDto>.Success(resumeDto);
        }

        public async Task<ServiceResult<IEnumerable<ResumeDto>>> GetResumesByUserIdAsync(Guid userId)
        {

            var resumes = await _resumeRepository.GetByUserIdAsync(userId);

            if (resumes == null || !resumes.Any())
            {
                return ServiceResult<IEnumerable<ResumeDto>>.Fail("Kullanıcıya ait özgeçmiş bulunamadı.", HttpStatusCode.NotFound);
            }

            var resumeDtos = _mapper.Map<IEnumerable<ResumeDto>>(resumes);
            return ServiceResult<IEnumerable<ResumeDto>>.Success(resumeDtos);
        }

        public async Task<ServiceResult> UpdateResumeAsync(Guid id, ResumeUpdateDto resumeUpdateDto)
        {
            var resume = await _resumeRepository.GetByIdAsync(id);
            if (resume == null)
            {
                return ServiceResult.Fail("Özgeçmiş bulunamadı.", HttpStatusCode.NotFound);
            }

            _mapper.Map(resumeUpdateDto, resume);
            resume.UpdatedAt = DateTime.Now;

            _resumeRepository.Update(resume);

            await _unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult<ResumeDto>> GetResumeByJobApplicationIdAsync(Guid jobApplicationId)
        {
            var jobApplication = await _jobApplicationRepository.GetByIdAsync(jobApplicationId);

            if (jobApplication == null)
            {
                return ServiceResult<ResumeDto>.Fail("Başvuru bulunamadı.", HttpStatusCode.NotFound);
            }

            if (!jobApplication.ResumeId.HasValue)
            {
                return ServiceResult<ResumeDto>.Fail("Başvuruya ait özgeçmiş bulunamadı.", HttpStatusCode.NotFound);
            }

            var resume = await _resumeRepository.GetByIdAsync(jobApplication.ResumeId.Value);

            if (resume == null)
            {
                return ServiceResult<ResumeDto>.Fail("Başvuruya ait özgeçmiş bulunamadı.", HttpStatusCode.NotFound);
            }

            var resumeDto = _mapper.Map<ResumeDto>(resume);

            return ServiceResult<ResumeDto>.Success(resumeDto);
        }
    }
}
