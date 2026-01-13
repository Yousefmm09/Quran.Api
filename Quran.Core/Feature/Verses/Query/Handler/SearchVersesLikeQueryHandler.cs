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
    public class SearchVersesLikeQueryHandler : IRequestHandler<SearchVersesLikeQuery, ApiResponse<List<VersesDto>>>
    {
        private readonly IVerses _verses;
        public SearchVersesLikeQueryHandler(IVerses verses)
        {
            _verses = verses;
        }
        public async Task<ApiResponse<List<VersesDto>>> Handle(SearchVersesLikeQuery request, CancellationToken cancellationToken)
        {
            var response =  await _verses.SearchVersesLikeAsync(request.SearchText);
            return response;
        }
    }
}
