using AutoMapper;
using Bio.Application.Dtos.JobPost;
using Bio.Application.Dtos.News;
using Bio.Application.Interfaces;
using Bio.Domain.Entities;
using Bio.Domain.Interfaces;
using Bio.Domain.Repositories;
using BlogProject.Application.Result;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Reflection.Metadata;
using System.Security.Claims;

namespace Bio.Application.Services
{
    public class NewsService : INewsService
    {
        private readonly INewsRepository _newsRepository;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;

        public NewsService(INewsRepository newsRepository, IMapper mapper, IPhotoService photoService, IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork)
        {
            _newsRepository = newsRepository;
            _mapper = mapper;
            _photoService = photoService;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
        }
        private Guid GetAdminIdFromToken()
        {
            var adminIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
            if (adminIdClaim == null || !Guid.TryParse(adminIdClaim.Value, out var adminId))
            {
                throw new UnauthorizedAccessException("Admin kimliği doğrulanamadı veya geçersiz.");
            }

            return adminId;
        }

        public async Task<ServiceResult<NewsDto>> CreateNewsAsync(NewsCreateDto newsCreateDto, IFormFile formFile)
        {
           var adminId = GetAdminIdFromToken();

            string photoPath = null;
            if(formFile != null)
            {
                photoPath = await _photoService.SavePhotoAsync(formFile, "News", Guid.NewGuid().ToString());
            }

            var news = _mapper.Map<News>(newsCreateDto);
            news.AdminId = adminId;
            news.ImagePath = photoPath;
            news.CreatedAt = DateTime.Now;

            await _newsRepository.AddAsync(news);

            var newsDto = _mapper.Map<NewsDto>(news);

            await _unitOfWork.SaveChangesAsync();

            return ServiceResult<NewsDto>.SuccessAsCreated(newsDto);

        }

        public async Task<ServiceResult> DeleteNewsAsync(Guid newsId)
        {
            var news = await _newsRepository.GetByIdAsync(newsId);

            if(news == null)
            {
                return ServiceResult.Fail("Haber bulunamadı.", HttpStatusCode.NotFound);
            }

            if (!string.IsNullOrEmpty(news.ImagePath))
            {
                _photoService.DeletePhoto(news.ImagePath);
            }
            _newsRepository.Delete(news);

            await _unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult<IEnumerable<NewsDto>>> GetAll()
        {
            var news = await _newsRepository.GetAllAsync();
            if (news == null || !news.Any()) 
            {
                return ServiceResult<IEnumerable<NewsDto>>.Fail("Henüz kayıtlı bir Haber bulunamadı");
            }

            var newsDto = _mapper.Map<IEnumerable<NewsDto>>(news);

            return ServiceResult<IEnumerable<NewsDto>>.Success(newsDto);
        }

        public async Task<ServiceResult<NewsDto>> GetNewsByIdAsync(Guid id)
        {
            var news = await _newsRepository.GetByIdAsync(id);

            if(news == null)
            {
                ServiceResult<NewsDto>.Fail("Haber bulunamadı", HttpStatusCode.NotFound);
            }

            var newsDto = _mapper.Map<NewsDto>(news);

            return ServiceResult<NewsDto>.Success(newsDto);
        }

        public async Task<ServiceResult<IEnumerable<NewsDto>>> GetNewsPagedAsync(int pageNumber, int pageSize)
        {
            var (pagedNews, totalItems) = await _newsRepository.GetAllNewsPagedAsync(pageNumber, pageSize);
            if (pagedNews == null || !pagedNews.Any())
            {
                return ServiceResult<IEnumerable<NewsDto>>.Fail("Henüz kayıtlı bir haber bulunamadı.");
            }

            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            var newsDtos = _mapper.Map<IEnumerable<NewsDto>>(pagedNews);

            return ServiceResult<IEnumerable<NewsDto>>.SuccessWithPagination(newsDtos, totalItems, totalPages);
        }

        public async Task<ServiceResult> UpdateNewsAsync(Guid newsId, NewsUpdateDto newsUpdateDto, IFormFile formFile)
        {
            var news = await _newsRepository.GetByIdAsync(newsId);
            if(news == null)
            {
                ServiceResult<NewsDto>.Fail("Haber bulunamadı", HttpStatusCode.NotFound);
            }

            if (formFile != null)
            {
                if (!string.IsNullOrEmpty(news.ImagePath))
                {
                    _photoService.DeletePhoto(news.ImagePath);
                }

                var photoPath = await _photoService.SavePhotoAsync(formFile, "News", Guid.NewGuid().ToString());
                news.ImagePath = photoPath;
            }
            news.UpdatedAt = DateTime.Now;

            _mapper.Map(newsUpdateDto, news);

            await _unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }
    }
}
