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
    public class StopVerseQueryHandler : IRequestHandler<StopVerseQuery, ApiResponse<StopVerseDto>>
    {
        private readonly IVerses _versesService;

        public StopVerseQueryHandler(IVerses versesService)
        {
            _versesService = versesService;
        }

        public async Task<ApiResponse<StopVerseDto>> Handle(StopVerseQuery request, CancellationToken cancellationToken)
        {
            return await _versesService.StopVerse(request.VerseId);
        }
    }
}
