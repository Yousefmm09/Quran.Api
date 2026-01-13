using MediatR;
using Quran.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quran.Core.Feature.Verses.Query.Model
{
    public class GetVersesByIdQuery:IRequest<ApiResponse<VersesDto>>
    {
        public int Id { get; set; }
       
    }
}
