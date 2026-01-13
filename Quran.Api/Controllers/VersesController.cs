using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quran.Core.Feature.Verses.Query.Model;
using Quran.Services.Dto;
using System.Diagnostics.Contracts;

namespace Quran.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VersesController : ControllerBase
    {
        private readonly IMediator _mediatR;
        public VersesController(IMediator mediatR)
        {
            _mediatR = mediatR;
        }
        [HttpGet("{id}")]
        public Task<ApiResponse<VersesDto>> GetVerseByIdAsync(int id)
        {
            var query = new GetVersesByIdQuery { Id = id };
            return _mediatR.Send(query);
        }
        [HttpGet("surah/{surahId}/verse/{id}")]
        public Task<ApiResponse<VersesDto>> GetVersesBySurahNameAsync(int surahId, int id)
        {
            var query = new GetVersesBySurahName { SurahId = surahId, Id = id };
            return _mediatR.Send(query);
        }
        [HttpGet("search")]
        public Task<ApiResponse<List<VersesDto>>> SearchVersesAsync([FromQuery] string searchText)
        {
            var query = new SearchVersesQuery { SearchText = searchText };
            return _mediatR.Send(query);
        }
        [HttpGet("search-like")]
        public Task<ApiResponse<List<VersesDto>>> SearchVersesLikeAsync([FromQuery] string searchText)
        {
            var query = new SearchVersesLikeQuery { SearchText = searchText };
            return _mediatR.Send(query);
        }
        [HttpGet("search-pagination")]
        public Task<ApiResponse<VersesPaginationDto>> SearchVersesPaginationAsync([FromQuery] string searchText, int pageNumber, int pageSize)
        {
            var query = new SearchVersesPaginationQuery { SearchText = searchText, PageNumber = pageNumber, PageSize = pageSize };
            return _mediatR.Send(query);
        }
        [HttpPost("stop-verse/{verseId}")]
        public Task<ApiResponse<StopVerseDto>> StopVerseAsync(int verseId)
        {
            var command = new StopVerseQuery { VerseId = verseId };
            return _mediatR.Send(command);
        }

    }
}
