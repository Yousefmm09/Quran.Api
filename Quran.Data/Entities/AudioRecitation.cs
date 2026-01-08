using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quran.Data.Entities
{
    public class AudioRecitation
    {
        public int Id { get; set; }
        public int ReciterId { get; set; }
        public string ReciterNameAr { get; set; }
        public string ReciterNameEn { get; set; }
        public string RewayaAr { get; set; }
        public string RewayaEn { get; set; }
        public string Server { get; set; }
        public string Link { get; set; }

        // Foreign Key
        public int SurahId { get; set; }
        public Surah Surah { get; set; }
    }
}
