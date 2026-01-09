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
    public class GetSurahByIdQueryHandler : IRequestHandler<GetSurahByIdQuery, ApiResponse<SurahDto>?>
    {
        private readonly ISurahService _surahService;
        public GetSurahByIdQueryHandler(ISurahService surahService)
        {
            _surahService = surahService;
        }

        public Task<ApiResponse<SurahDto>?> Handle(GetSurahByIdQuery request, CancellationToken cancellationToken)
        {
            return _surahService.GetSurahByIdAsync(request.Id);
        }
    }
}
