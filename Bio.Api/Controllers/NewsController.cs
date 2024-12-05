using Bio.Application.Dtos.News;
using Bio.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bio.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : CustomBaseController
    {
        private readonly INewsService newsService;

        public NewsController(INewsService newsService)
        {
            this.newsService = newsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNews()
        {
            var result = await newsService.GetAll();
            return CreateActionResult(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetNewsById(Guid id)
        {
            var result = await newsService.GetNewsByIdAsync(id);
            return CreateActionResult(result);
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetNewsPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await newsService.GetNewsPagedAsync(pageNumber, pageSize);
            return CreateActionResult(result);
        }

        [HttpPost]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> CreateNews([FromForm] NewsCreateDto newsCreateDto, IFormFile newsImage)
        {
            var result = await newsService.CreateNewsAsync(newsCreateDto, newsImage);
            return CreateActionResult(result);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> UpdateNews(Guid id, [FromForm] NewsUpdateDto newsUpdateDto, IFormFile? newsImage)
        {
            var result = await newsService.UpdateNewsAsync(id, newsUpdateDto, newsImage);
            return CreateActionResult(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> DeleteNews(Guid id)
        {
            var result = await newsService.DeleteNewsAsync(id);
            return CreateActionResult(result);
        }
    }
}
