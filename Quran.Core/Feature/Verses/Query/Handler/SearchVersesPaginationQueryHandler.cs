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
    public class SearchVersesPaginationQueryHandler: IRequestHandler<SearchVersesPaginationQuery, ApiResponse<VersesPaginationDto>>
    {
        private readonly IVerses _verses;
        public SearchVersesPaginationQueryHandler(IVerses verses)
        {
            _verses = verses;
        }

        public async Task<ApiResponse<VersesPaginationDto>> Handle(SearchVersesPaginationQuery request, CancellationToken cancellationToken)
        {
            return await _verses.SearchVersesPaginationAsync(request.SearchText, request.PageNumber, request.PageSize);
        }
    }
}
