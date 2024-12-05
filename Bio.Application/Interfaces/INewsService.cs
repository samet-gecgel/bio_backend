using Bio.Application.Dtos.News;
using BlogProject.Application.Result;
using Microsoft.AspNetCore.Http;

namespace Bio.Application.Interfaces
{
    public interface INewsService
    {
        Task<ServiceResult<NewsDto>> GetNewsByIdAsync(Guid id);
        Task<ServiceResult<IEnumerable<NewsDto>>> GetAll();
        Task<ServiceResult<IEnumerable<NewsDto>>> GetNewsPagedAsync(int pageNumber, int pageSize);
        Task<ServiceResult<NewsDto>> CreateNewsAsync(NewsCreateDto newsCreateDto, IFormFile formFile);
        Task<ServiceResult> UpdateNewsAsync(Guid newsId, NewsUpdateDto newsUpdateDto, IFormFile formFile);
        Task<ServiceResult> DeleteNewsAsync(Guid newsId);
    }
}
