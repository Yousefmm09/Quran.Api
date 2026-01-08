using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Quran.Infrastructure.Seeder
{
    public class SajdaDto
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }

        [JsonPropertyName("recommended")]
        public bool Recommended { get; set; }

        [JsonPropertyName("obligatory")]
        public bool Obligatory { get; set; }

        // Helper to check if this verse has sajda
        public bool HasSajda => Id.HasValue && Id.Value > 0;

    }
}
