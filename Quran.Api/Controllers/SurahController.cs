using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quran.Core.Feature.Surahs.Query.Model;
using Quran.Services.Dto;

namespace Quran.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurahController : ControllerBase
    {
        private readonly ILogger<SurahController> _logger;
        private readonly IMediator _mediator;
        public SurahController(ILogger<SurahController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        [HttpGet]
        public Task<ApiResponse<List<SurahDto>>> GetAllSurahsAsync()
        {
            var query = new Quran.Core.Feature.Surahs.Query.Model.GetAllSurahQuery();
            return _mediator.Send(query);
        }
        [HttpGet("{id}")]
        public Task<ApiResponse<SurahDto>?> GetSurahByIdAsync(int id)
        {
            var query = new GetSurahByIdQuery { Id = id };
            return _mediator.Send(query);
        }
        [HttpGet("{id:int}/verses")]
        public async Task<ApiResponse<List<VersesDto>>> getVerseSurahbyId(int id)
        {
            var query =   new GetVersesSurahByIdQuery { Id=id};
            return  await _mediator.Send(query);
        }
    }
}
