using MediatR;
using Quran.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quran.Core.Feature.Audio.Query
{
    public class SearchRecitationandSurahNameQuery:IRequest<ApiResponse<List<SearchAudioRectiterbySurahIdDto>>>
    {
        public string Recitation { get; set; }
        public int SurahId { get; set; }
        
    
    }
}
