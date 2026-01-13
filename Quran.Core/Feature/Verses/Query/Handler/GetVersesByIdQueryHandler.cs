using MediatR;
using MediatR.Pipeline;
using Quran.Core.Feature.Verses.Query.Model;
using Quran.Services.Abstract;
using Quran.Services.Dto;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quran.Core.Feature.Verses.Query.Handler
{
    public class GetVersesByIdQueryHandler : IRequestHandler<GetVersesByIdQuery, ApiResponse<VersesDto>>
    {
        private readonly IVerses _versesService;
        public GetVersesByIdQueryHandler(IVerses versesService)
        {
            _versesService = versesService;
        }
        public Task<ApiResponse<VersesDto>> Handle(GetVersesByIdQuery request, CancellationToken cancellationToken)
        {
            return _versesService.Get(request.Id);
        }
    }
}
