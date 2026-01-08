using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Quran.Infrastructure.Seeder
{
    public class TafsirDto
    {
        [JsonPropertyName("number")]
        public int SurahNumber { get; set; }

        [JsonPropertyName("aya")]
        public int AyahNumber { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }

    }
}
