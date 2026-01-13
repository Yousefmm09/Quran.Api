using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quran.Services.Dto
{
    public class AudioRecitationDto
    {
        public int Id {  get; set; }
        public string ReciterNameAr {  get; set; }
        public string RewayaAr {  get; set; }
        public string SurahName {  get; set; }
        public string Server {  get; set; }
        public int SurahId { get; set; }
        public string Link { get; set; }
    }
}
