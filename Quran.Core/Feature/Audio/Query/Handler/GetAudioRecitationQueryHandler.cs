using MediatR;
using Quran.Core.Feature.Audio.Query.Model;
using Quran.Infrastructure.Abstract;
using Quran.Services.Abstract;
using Quran.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quran.Core.Feature.Audio.Query.Handler
{
    public class GetAudioRecitationQueryHandler : IRequestHandler<GetAudioRecitationQuery, ApiResponse<AudioRecitationDto>>
    {
        private readonly IAudioService _audioService;

        public GetAudioRecitationQueryHandler(IAudioService audioService)
        {
            _audioService = audioService;
        }

        public async Task<ApiResponse<AudioRecitationDto>> Handle(GetAudioRecitationQuery request, CancellationToken cancellationToken)
        {
            var audioRecitation = await _audioService.GetAudioRecitation(request.Id);
            return audioRecitation;
        }
    }
}
