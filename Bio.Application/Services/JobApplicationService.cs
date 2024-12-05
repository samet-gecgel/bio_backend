using AutoMapper;
using Bio.Application.Dtos.Certificate;
using Bio.Application.Dtos.JobApplication;
using Bio.Application.Interfaces;
using Bio.Domain.Entities;
using Bio.Domain.Enums;
using Bio.Domain.Interfaces;
using Bio.Domain.Repositories;
using BlogProject.Application.Result;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Security.Claims;

namespace Bio.Application.Services
{
    public class JobApplicationService : IJobApplicationService
    {
        private readonly IJobApplicationRepository _jobApplicationRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;

        public JobApplicationService(IJobApplicationRepository jobApplicationRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork)
        {
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

        public async Task<ServiceResult<JobApplicationDto>> CreateJobApplicationAsync(JobApplicationCreateDto jobApplicationCreateDto)
        {
            var userId = GetUserIdFromToken();
            var existingJobApplication = await _jobApplicationRepository.FindAsync(ja =>
                 ja.UserId == userId && ja.JobPostId == jobApplicationCreateDto.JobPostId);

            if (existingJobApplication != null)
            {
                return ServiceResult<JobApplicationDto>.Fail("Bu ilana daha önce başvurulmuştur.");
            }
            var jobApplication = _mapper.Map<JobApplication>(jobApplicationCreateDto);
            jobApplication.UserId = userId;
            jobApplication.Status = ApplicationStatus.Pending;
            jobApplication.ApplicationDate = DateTime.Now;

            await _jobApplicationRepository.AddAsync(jobApplication);

            var jobApplicationDto = _mapper.Map<JobApplicationDto>(jobApplication);

            await _unitOfWork.SaveChangesAsync();

            return ServiceResult<JobApplicationDto>.Success(jobApplicationDto);
        }

        public async Task<ServiceResult> DeleteJobApplicationAsync(Guid id)
        {
            var jobApplication = await _jobApplicationRepository.GetByIdAsync(id);
            if (jobApplication == null)
            {
                return ServiceResult.Fail("İş başvurusu bulunamadı", HttpStatusCode.NotFound);
            }

            _jobApplicationRepository.Delete(jobApplication);

            await _unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult<IEnumerable<JobApplicationDto>>> GetAllJobApplicationsAsync()
        {
            var jobApplications = await _jobApplicationRepository.GetAllAsync();
            if (jobApplications == null || !jobApplications.Any())
            {
                return ServiceResult<IEnumerable<JobApplicationDto>>.Fail("Henüz kayıtlı bir iş başvurusu bulunamadı.");
            }

            var jobApplicationDtos = _mapper.Map<IEnumerable<JobApplicationDto>>(jobApplications);

            return ServiceResult<IEnumerable<JobApplicationDto>>.Success(jobApplicationDtos);
        }

        public async Task<ServiceResult<JobApplicationDto>> GetJobApplicationByIdAsync(Guid id)
        {
            var jobApplication = await _jobApplicationRepository.GetByIdAsync(id);
            if (jobApplication == null)
            {
                return ServiceResult<JobApplicationDto>.Fail("İş başvurusu bulunamadı.", HttpStatusCode.NotFound);
            }

            var jobApplicationDto = _mapper.Map<JobApplicationDto>(jobApplication);
            return ServiceResult<JobApplicationDto>.Success(jobApplicationDto);
        }

        public async Task<ServiceResult<IEnumerable<JobApplicationDto>>> GetJobApplicationsByJobPostIdAsync(Guid jobPostId)
        {
            var jobApplications = await _jobApplicationRepository.GetByJobPostIdAsync(jobPostId);
            if (jobApplications == null || !jobApplications.Any())
            {
                return ServiceResult<IEnumerable<JobApplicationDto>>.Fail("Bu ilana ait iş başvurusu bulunamadı.", HttpStatusCode.NotFound);
            }

            var jobApplicationDtos = _mapper.Map<IEnumerable<JobApplicationDto>>(jobApplications);
            return ServiceResult<IEnumerable<JobApplicationDto>>.Success(jobApplicationDtos);
        }

        public async Task<ServiceResult<IEnumerable<JobApplicationDto>>> GetJobApplicationsByUserIdAsync()
        {
            var userId = GetUserIdFromToken();

            var jobApplications = await _jobApplicationRepository.GetByUserIdAsync(userId);
            if (jobApplications == null || !jobApplications.Any())
            {
                return ServiceResult<IEnumerable<JobApplicationDto>>.Fail("Henüz yaptığınız iş başvurusu bulunamadı.", HttpStatusCode.NotFound);
            }

            var jobApplicationDtos = _mapper.Map<IEnumerable<JobApplicationDto>>(jobApplications);
            return ServiceResult<IEnumerable<JobApplicationDto>>.Success(jobApplicationDtos);
        }

        public async Task<ServiceResult> UpdateJobApplicationAsync(Guid id, JobApplicationUpdateDto jobApplicationUpdateDto)
        {
            var jobApplication = await _jobApplicationRepository.GetByIdAsync(id);
            if (jobApplication == null)
            {
                return ServiceResult.Fail("İş başvurusu bulunamadı.", HttpStatusCode.NotFound);
            }

            _mapper.Map(jobApplicationUpdateDto, jobApplication);
            _jobApplicationRepository.Update(jobApplication);

            await _unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult> ApproveJobApplicationAsync(Guid id)
        {
            var jobApplication = await _jobApplicationRepository.GetByIdAsync(id);

            if (jobApplication == null)
            {
                return ServiceResult.Fail("İş başvurusu bulunamadı.", HttpStatusCode.NotFound);
            }

            if (jobApplication.Status == ApplicationStatus.Approved)
            {
                return ServiceResult.Fail("İş başvurusu zaten onaylanmış.", HttpStatusCode.BadRequest);
            }

            jobApplication.Status = ApplicationStatus.Approved;
            jobApplication.UpdatedAt = DateTime.UtcNow;

            _jobApplicationRepository.Update(jobApplication);

            await _unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult> RejectJobApplicationAsync(Guid id)
        {
            var jobApplication = await _jobApplicationRepository.GetByIdAsync(id);

            if (jobApplication == null)
            {
                return ServiceResult.Fail("İş başvurusu bulunamadı.", HttpStatusCode.NotFound);
            }

            if (jobApplication.Status == ApplicationStatus.Rejected)
            {
                return ServiceResult.Fail("İş başvurusu zaten reddedilmiş.", HttpStatusCode.BadRequest);
            }

            jobApplication.Status = ApplicationStatus.Rejected;
            jobApplication.UpdatedAt = DateTime.UtcNow;

            _jobApplicationRepository.Update(jobApplication);

            await _unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult<IEnumerable<JobApplicationDto>>> GetApplicationsByCompanyIdAsync(Guid companyId)
        {
            var applications = await _jobApplicationRepository.GetApplicationsByCompanyIdAsync(companyId);

            if (!applications.Any())
            {
                return ServiceResult<IEnumerable<JobApplicationDto>>.Fail("Hiçbir başvuru bulunamadı.", HttpStatusCode.NotFound);
            }

            var applicationDtos = _mapper.Map<IEnumerable<JobApplicationDto>>(applications);

            return ServiceResult<IEnumerable<JobApplicationDto>>.Success(applicationDtos);
        }

        public async Task<ServiceResult<JobApplicationDetailDto>> GetApplicationDetailByIdAsync(Guid applicationId)
        {
            var application = await _jobApplicationRepository.GetApplicationDetailByIdAsync(applicationId);

            if (application == null)
            {
                return ServiceResult<JobApplicationDetailDto>.Fail("Başvuru bulunamadı.", HttpStatusCode.NotFound);
            }

            var applicationDetailDto = _mapper.Map<JobApplicationDetailDto>(application);

            return ServiceResult<JobApplicationDetailDto>.Success(applicationDetailDto);
        }




    }
}
