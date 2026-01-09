using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quran.Services.Dto
{
    public class VersesDto
    {
        public int Id { get; set; }
        public string TextAr { get; set; }
        public int VersesNumber { get; set; }
        public IQueryable<string> SurahName {  get; set; }
    }
}
