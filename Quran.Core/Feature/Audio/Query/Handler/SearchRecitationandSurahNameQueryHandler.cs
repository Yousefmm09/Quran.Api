using MediatR;
using Quran.Services.Abstract;
using Quran.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quran.Core.Feature.Audio.Query.Handler
{
    public class SearchRecitationandSurahNameQueryHandler: IRequestHandler<SearchRecitationandSurahNameQuery, ApiResponse<List<SearchAudioRectiterbySurahIdDto>>>
    {
        private readonly IAudioService _audioService;
        public SearchRecitationandSurahNameQueryHandler(IAudioService audioService)
        {
            _audioService = audioService;
        }

        public async Task<ApiResponse<List<SearchAudioRectiterbySurahIdDto>>> Handle(SearchRecitationandSurahNameQuery request, CancellationToken cancellationToken)
        {
            return await _audioService.SearchRecitationandSurahName(request.Recitation, request.SurahId);
        }
    }
}
