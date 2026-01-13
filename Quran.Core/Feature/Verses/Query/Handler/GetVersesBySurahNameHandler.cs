using MediatR;
using Quran.Core.Feature.Verses.Query.Model;
using Quran.Services.Abstract;
using Quran.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quran.Core.Feature.Verses.Query.Handler
{
    public class GetVersesBySurahNameHandler : IRequestHandler<GetVersesBySurahName, ApiResponse<VersesDto>>
    {
        private readonly IVerses _versesService;
        public GetVersesBySurahNameHandler(IVerses verses)
        {
         _versesService = verses;   
        }
        public Task<ApiResponse<VersesDto>> Handle(GetVersesBySurahName request, CancellationToken cancellationToken)
        {
            return _versesService.GetVerseBySurahName(request.Id, request.SurahId);
        }
    }
}
