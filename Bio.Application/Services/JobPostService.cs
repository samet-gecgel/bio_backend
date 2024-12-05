using AutoMapper;
using Bio.Application.Dtos.JobPost;
using Bio.Application.Interfaces;
using Bio.Domain.Entities;
using Bio.Domain.Enums;
using Bio.Domain.Interfaces;
using Bio.Domain.Repositories;
using BlogProject.Application.Result;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;
using System.Net;
using System.Security.Claims;

namespace Bio.Application.Services
{
    public class JobPostService : IJobPostService
    {
        private readonly IJobPostRepository _jobPostRepository;
        private readonly IJobCategoryRepository _jobCategoryRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JobPostService(
            IJobPostRepository jobPostRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IHttpContextAccessor httpContextAccessor,
            IJobCategoryRepository jobCategoryRepository,
            ICompanyRepository companyRepository)
        {
            _jobPostRepository = jobPostRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _jobCategoryRepository = jobCategoryRepository;
            _companyRepository = companyRepository;
        }

        private Guid GetCompanyIdFromToken()
        {
            var companyIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
            if (companyIdClaim == null || !Guid.TryParse(companyIdClaim.Value, out var companyId))
            {
                throw new UnauthorizedAccessException("Şirket kimliği doğrulanamadı veya geçersiz.");
            }

            return companyId;
        }

        public async Task<ServiceResult<JobPostDto>> CreateJobPostAsync(JobPostCreateDto jobPostCreateDto)
        {

            var jobPost = _mapper.Map<JobPost>(jobPostCreateDto);
            jobPost.CompanyId = GetCompanyIdFromToken();

            jobPost.IsActive = true;
            jobPost.ApplicationStatus = ApplicationStatus.Pending;
            jobPost.PublishedDate = DateTime.UtcNow;

            await _jobPostRepository.AddAsync(jobPost);

            var jobPostDto = _mapper.Map<JobPostDto>(jobPost);

            await _unitOfWork.SaveChangesAsync();

            return ServiceResult<JobPostDto>.SuccessAsCreated(jobPostDto);
        }

        public async Task<ServiceResult> DeleteJobPostAsync(Guid id)
        {
            var jobPost = await _jobPostRepository.GetByIdAsync(id);
            if (jobPost == null)
            {
                return ServiceResult.Fail("İş ilanı bulunamadı.", HttpStatusCode.NotFound);
            }

            _jobPostRepository.Delete(jobPost);

            await _unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult<IEnumerable<JobPostDto>>> GetActiveJobPostAsync()
        {
            var activeJobPosts = await _jobPostRepository.GetActiveJobPostAsync();
            if (activeJobPosts == null || !activeJobPosts.Any())
            {
                return ServiceResult<IEnumerable<JobPostDto>>.Fail("Aktif iş ilanı bulunamadı.");
            }

            var activeJobPostDtos = _mapper.Map<IEnumerable<JobPostDto>>(activeJobPosts);

            return ServiceResult<IEnumerable<JobPostDto>>.Success(activeJobPostDtos);
        }

        public async Task<ServiceResult<IEnumerable<JobPostDto>>> GetAllJobPostsAsync()
        {
            var jobPosts = await _jobPostRepository.GetAllAsync();
            if (jobPosts == null || !jobPosts.Any())
            {
                return ServiceResult<IEnumerable<JobPostDto>>.Fail("Henüz iş ilanı bulunamadı.");
            }

            var jobPostDtos = _mapper.Map<IEnumerable<JobPostDto>>(jobPosts);

            return ServiceResult<IEnumerable<JobPostDto>>.Success(jobPostDtos);
        }

        public async Task<ServiceResult<JobPostDto>> GetJobPostByIdAsync(Guid id)
        {
            var jobPost = await _jobPostRepository.GetJobPostWithApplicationsAsync(id);
            if (jobPost == null)
            {
                return ServiceResult<JobPostDto>.Fail("İş ilanı bulunamadı.", HttpStatusCode.NotFound);
            }

            jobPost.ViewCount++;
            await _unitOfWork.SaveChangesAsync();

            var jobPostDto = _mapper.Map<JobPostDto>(jobPost);

            return ServiceResult<JobPostDto>.Success(jobPostDto);
        }

        public async Task<ServiceResult<IEnumerable<JobPostDto>>> GetPagedJobPostsAsync(int pageNumber, int pageSize, JobPostFilterDto filter)
        {
            Expression<Func<JobPost, bool>> predicate = jp =>
                jp.IsActive && 
                jp.ApplicationStatus == ApplicationStatus.Approved && 
                (string.IsNullOrEmpty(filter.Title) || jp.Title.Contains(filter.Title)) &&
                (filter.Districts == null || !filter.Districts.Any() || filter.Districts.Contains(jp.District)) &&
                (filter.StartDate == null || jp.PublishedDate >= filter.StartDate) &&
                (filter.EndDate == null || jp.PublishedDate <= filter.EndDate) &&
                (filter.OffDays == null || (jp.OffDays & filter.OffDays) != 0) &&
                (filter.RequiredEducationLevel == null || (jp.RequiredEducationLevel & filter.RequiredEducationLevel) != 0) &&
                (filter.JobTypes == null || (jp.JobType & filter.JobTypes) != 0) &&
                (filter.ExperienceLevel == null || (jp.ExperienceLevel & filter.ExperienceLevel) != 0) &&
                (filter.RequiresDrivingLicense == null || jp.RequiresDrivingLicense == filter.RequiresDrivingLicense) &&
                (filter.IsDisabledFriendly == null || jp.IsDisabledFriendly == filter.IsDisabledFriendly);

            var totalItems = await _jobPostRepository.CountAsync(predicate);

            var jobPosts = await _jobPostRepository.GetPagedJobPostsAsync(pageNumber, pageSize, predicate);

            var jobPostDtos = _mapper.Map<IEnumerable<JobPostDto>>(jobPosts);

            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            return ServiceResult<IEnumerable<JobPostDto>>.SuccessWithPagination(jobPostDtos, totalItems, totalPages);
        }



        public async Task<ServiceResult<IEnumerable<JobPostDto>>> GetJobPostsByCategoryIdAsync(Guid categoryId)
        {
            var jobPosts = await _jobPostRepository.GetByCategoryIdAsync(categoryId);
            if (jobPosts == null || !jobPosts.Any())
            {
                return ServiceResult<IEnumerable<JobPostDto>>.Fail("Bu kategoriye ait iş ilanı bulunamadı.");
            }

            var jobPostDtos = _mapper.Map<IEnumerable<JobPostDto>>(jobPosts);

            return ServiceResult<IEnumerable<JobPostDto>>.Success(jobPostDtos);
        }

        public async Task<ServiceResult<IEnumerable<JobPostDto>>> GetJobPostsByCompanyIdAsync()
        {
            var companyId = GetCompanyIdFromToken();
            var jobPosts = await _jobPostRepository.GetByCompanyIdAsync(companyId);
            if (jobPosts == null || !jobPosts.Any())
            {
                return ServiceResult<IEnumerable<JobPostDto>>.Fail("Şirketinize ait iş ilanı bulunamadı.");
            }

            var jobPostDtos = _mapper.Map<IEnumerable<JobPostDto>>(jobPosts);

            return ServiceResult<IEnumerable<JobPostDto>>.Success(jobPostDtos);
        }

        public async Task<ServiceResult<IEnumerable<JobPostDto>>> GetJobPostsByCompanyIdPagedAsync(int pageNumber, int pageSize)
        {
            var companyId = GetCompanyIdFromToken();
            
            var (pagedJobPosts, totalItems) = await _jobPostRepository.GetByCompanyIdPagedAsync(companyId, pageNumber, pageSize);

            if(pagedJobPosts == null || !pagedJobPosts.Any())
            {
                return ServiceResult<IEnumerable<JobPostDto>>.Fail("Henüz şirketinize ait bir iş ilanı bulunamadı.", HttpStatusCode.NotFound);
            }

            var jobPostDto = _mapper.Map<IEnumerable<JobPostDto>>(pagedJobPosts);

            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            return ServiceResult<IEnumerable<JobPostDto>>.SuccessWithPagination(jobPostDto, totalItems, totalPages);
        }

        public async Task<ServiceResult<IEnumerable<JobPostDto>>> GetJobPostsPagedAsync(int pageNumber, int pageSize)
        {
            var (pagedJobPosts, totalItems) = await _jobPostRepository.GetAllJobPostPagedAsync(pageNumber, pageSize);
            if (pagedJobPosts == null || !pagedJobPosts.Any())
            {
                return ServiceResult<IEnumerable<JobPostDto>>.Fail("Henüz kayıtlı bir iş ilanı bulunamadı.");
            }

            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            var jobPostDtos = _mapper.Map<IEnumerable<JobPostDto>>(pagedJobPosts);

            return ServiceResult<IEnumerable<JobPostDto>>.SuccessWithPagination(jobPostDtos, totalItems, totalPages);
        }
        public async Task<ServiceResult> ApproveJobPostAsync(Guid id)
        {
            var jobPost = await _jobPostRepository.GetByIdAsync(id);
            if (jobPost == null)
            {
                return ServiceResult.Fail("İş ilanı bulunamadı", HttpStatusCode.NotFound);
            }

            jobPost.ApplicationStatus = ApplicationStatus.Approved;
            _jobPostRepository.Update(jobPost);

            await _unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }
        public async Task<ServiceResult> RejectJobPostAsync(Guid id)
        {
            var jobPost = await _jobPostRepository.GetByIdAsync(id);
            if (jobPost == null)
            {
                return ServiceResult.Fail("İş ilanı bulunamadı", HttpStatusCode.NotFound);
            }

            jobPost.ApplicationStatus = ApplicationStatus.Rejected;
            _jobPostRepository.Update(jobPost);

            await _unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }
        public async Task<ServiceResult> UpdateJobPostAsync(Guid id, JobPostUpdateDto jobPostUpdateDto)
        {
            var jobPost = await _jobPostRepository.GetByIdAsync(id);
            if (jobPost == null)
            {
                return ServiceResult.Fail("İş ilanı bulunamadı.", HttpStatusCode.NotFound);
            }

            _mapper.Map(jobPostUpdateDto, jobPost);

            _jobPostRepository.Update(jobPost);

            await _unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }
        public async Task<ServiceResult<IEnumerable<JobPostDto>>> GetLatestJobPostsAsync(int count = 4)
        {
            var jobPosts = await _jobPostRepository.GetLatestJobPostsAsync(count);

            if (!jobPosts.Any())
            {
                return ServiceResult<IEnumerable<JobPostDto>>.Fail("Son iş ilanları bulunamadı.", HttpStatusCode.NotFound);
            }

            var jobPostDtos = _mapper.Map<IEnumerable<JobPostDto>>(jobPosts);
            return ServiceResult<IEnumerable<JobPostDto>>.Success(jobPostDtos);
        }

    }
}
