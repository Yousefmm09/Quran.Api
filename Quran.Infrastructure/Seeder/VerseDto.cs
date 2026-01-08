using System.Text.Json.Serialization;

namespace Quran.Infrastructure.Seeder
{
    public class VerseDto
    {
        [JsonPropertyName("number")]
        public int Number { get; set; }

        [JsonPropertyName("text")]
        public TextDto Text { get; set; }

        [JsonPropertyName("juz")]
        public int Juz { get; set; }

        [JsonPropertyName("page")]
        public int Page { get; set; }

        [JsonPropertyName("sajda")]
        [JsonConverter(typeof(SajdaFlexibleConverter))]
        public SajdaDto Sajda { get; set; }
    }
}
