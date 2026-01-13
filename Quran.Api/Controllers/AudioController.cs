using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quran.Core.Feature.Audio.Query;
using Quran.Core.Feature.Audio.Query.Model;

namespace Quran.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AudioController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AudioController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAudioRecitation(int id)
        {
            var query = new GetAudioRecitationQuery
            {
                Id = id
            };
            var result = await _mediator.Send(query);
            if (result.Success)
            {
                return Ok(result);
            }
            return NotFound(result);
        }
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllAudioRecitations(int pageNumber,int pageSize)
        {
            var query = new GetAudioRecitationListQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            var result = await _mediator.Send(query);
            if (result.Success)
            {
                return Ok(result);
            }
            return NotFound(result);
        }
        [HttpGet("search/{recitation}/{surahId}")]
        public async Task<IActionResult> SearchAudioRecitations(string recitation, int surahId)
        {
            var query = new SearchRecitationandSurahNameQuery
            {
                Recitation = recitation,
                SurahId = surahId
            };
            var result = await _mediator.Send(query);
            if (result.Success)
            {
                return Ok(result);
            }
            return NotFound(result);
        }
    }
}
