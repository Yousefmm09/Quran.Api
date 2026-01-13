using MediatR;
using Quran.Core.Feature.Audio.Query.Model;
using Quran.Services.Abstract;
using Quran.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quran.Core.Feature.Audio.Query.Handler
{
    public class GetAudioRecitationListQueryHandler: IRequestHandler<GetAudioRecitationListQuery, ApiResponse<List<AudioRecitationDto>>>
    {
        private readonly IAudioService _audioService;
        public GetAudioRecitationListQueryHandler(IAudioService audioService)
        {
            _audioService = audioService;
        }
        public async Task<ApiResponse<List<AudioRecitationDto>>> Handle(GetAudioRecitationListQuery request, CancellationToken cancellationToken)
        {
            var audioRecitations = await _audioService.GetAudioRecitationsAsync(request.PageNumber, request.PageSize);
            return audioRecitations;
        }
    }
}
