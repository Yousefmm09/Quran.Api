using MediatR;
using Quran.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quran.Core.Feature.Audio.Query.Model
{
    public class GetAudioRecitationQuery:IRequest<ApiResponse<AudioRecitationDto>>
    {
        public int Id { get; set; }
    }
}
