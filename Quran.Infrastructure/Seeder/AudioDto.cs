using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Quran.Infrastructure.Seeder
{
    public class AudioDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("reciter")]
        public ReciterDto Reciter { get; set; }

        [JsonPropertyName("rewaya")]
        public RewayaDto Rewaya { get; set; }

        [JsonPropertyName("server")]
        public string Server { get; set; }

        [JsonPropertyName("link")]
        public string Link { get; set; }
    }
}
