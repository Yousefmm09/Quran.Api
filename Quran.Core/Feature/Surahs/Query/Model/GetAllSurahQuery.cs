using MediatR;
using Quran.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quran.Core.Feature.Surahs.Query.Model
{
    public class GetAllSurahQuery:IRequest<ApiResponse<List<SurahDto>>>
    {
    }
}
