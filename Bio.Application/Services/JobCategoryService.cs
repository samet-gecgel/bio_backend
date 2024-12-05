using AutoMapper;
using Bio.Application.Dtos.JobCategory;
using Bio.Application.Interfaces;
using Bio.Domain.Entities;
using Bio.Domain.Interfaces;
using Bio.Domain.Repositories;
using BlogProject.Application.Result;
using System.Net;

namespace Bio.Application.Services
{
    public class JobCategoryService : IJobCategoryService
    {
        private readonly IJobCategoryRepository _jobCategoryRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public JobCategoryService(IJobCategoryRepository jobCategoryRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _jobCategoryRepository = jobCategoryRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult<JobCategoryDto>> CreateJobCategoryAsync(JobCategoryCreateDto jobCategoryCreateDto)
        {
            var existingJobCategory = await _jobCategoryRepository.GetCategoryByNameAsync(jobCategoryCreateDto.Name);
            if (existingJobCategory != null) 
            {
                return ServiceResult<JobCategoryDto>.Fail("Bu kategori daha önce oluşturulmuştur.");
            }

            var jobCategory = _mapper.Map<JobCategory>(jobCategoryCreateDto);

            await _jobCategoryRepository.AddAsync(jobCategory);

            var jobCategoryDto = _mapper.Map<JobCategoryDto>(jobCategory);

            await _unitOfWork.SaveChangesAsync();

            return ServiceResult<JobCategoryDto>.SuccessAsCreated(jobCategoryDto);
        }

        public async Task<ServiceResult> DeleteJobCategoryAsync(Guid id)
        {
            var jobCategory = await _jobCategoryRepository.GetByIdAsync(id);
            if (jobCategory == null) 
            {
                return ServiceResult.Fail("Kategori bulunamadı", HttpStatusCode.NotFound);
            }

            _jobCategoryRepository.Delete(jobCategory);

            await _unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);


        }

        public async Task<ServiceResult<IEnumerable<JobCategoryDto>>> GetAllJobCategoriesAsync()
        {
            var jobCategories = await _jobCategoryRepository.GetAllAsync();
            if(jobCategories == null || !jobCategories.Any())
            {
                return ServiceResult<IEnumerable<JobCategoryDto>>.Fail("Henüz bir kategori bulunamadı.");
            }

            var jobCategoryDtos = _mapper.Map<IEnumerable<JobCategoryDto>>(jobCategories);

            return ServiceResult<IEnumerable<JobCategoryDto>>.Success(jobCategoryDtos);
        }

        public async Task<ServiceResult<JobCategoryDto>> GetJobCategoryByIdAsync(Guid id)
        {
            var jobCategory = await _jobCategoryRepository.GetByIdAsync(id);
            if(jobCategory == null)
            {
                return ServiceResult<JobCategoryDto>.Fail("kategori bulunamadı", HttpStatusCode.NotFound);
            }

            var jobApplicationDto = _mapper.Map<JobCategoryDto>(jobCategory);
            return ServiceResult<JobCategoryDto>.Success(jobApplicationDto);
        }

        public async Task<ServiceResult> UpdateJobCategoryAsync(Guid id, JobCategoryUpdateDto jobCategoryUpdateDto)
        {
            var jobCategory = await _jobCategoryRepository.GetByIdAsync(id);
            if (jobCategory == null)
            {
                return ServiceResult.Fail("kategori bulunamadı", HttpStatusCode.NotFound);
            }

            _mapper.Map(jobCategoryUpdateDto, jobCategory);
            _jobCategoryRepository.Update(jobCategory);

            await _unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);

        }
    }
}
