using MediatR;
using Quran.Core.Feature.Surahs.Query.Model;
using Quran.Services.Abstract;
using Quran.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quran.Core.Feature.Surahs.Query.Handler
{
    public class GetVersesSurahByIdQueryHandler : IRequestHandler<GetVersesSurahByIdQuery, ApiResponse<List<VersesDto>>>
    {
        private readonly ISurahService _surah;
        public GetVersesSurahByIdQueryHandler(ISurahService surah)
        {
            _surah = surah;
        }
        public async Task <ApiResponse<List<VersesDto>>> Handle(GetVersesSurahByIdQuery request, CancellationToken cancellationToken)
        {
            var verses =  await _surah.GetVersesBySurahId(request.Id);
            return verses;
        }
    }
}
