using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quran.Services.Dto
{
    public class SurahDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ArabicName { get; set; } = string.Empty;
        public int NumberOfSurah{ get; set; }
        public string RevelationType { get; set; } = string.Empty;
        public int VerserCount { get; set; }
    }
}
