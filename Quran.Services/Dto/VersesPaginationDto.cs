using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quran.Services.Dto
{
    public class VersesPaginationDto
    {
        public List<VersesDto> Verses { get; set; }
        public int TotalCount { get; set; }
    }
}
