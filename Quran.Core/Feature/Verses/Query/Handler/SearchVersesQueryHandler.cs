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
    public class SearchVersesQueryHandler : IRequestHandler<SearchVersesQuery, ApiResponse<List<VersesDto>>>
    {
        private readonly IVerses _verses;

        public SearchVersesQueryHandler(IVerses versesRepo)
        {
            _verses = versesRepo;
        }

        public async Task<ApiResponse<List<VersesDto>>> Handle(SearchVersesQuery request, CancellationToken cancellationToken)
        {
            var verses = await _verses.SearchVersesAsync(request.SearchText);
            return verses;

        }
    }
}
