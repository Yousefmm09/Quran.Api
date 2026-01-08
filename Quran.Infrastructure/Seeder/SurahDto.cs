using System.Text.Json.Serialization;

namespace Quran.Infrastructure.Seeder
{
    public class SurahDto
    {

        [JsonPropertyName("number")]
        public int Number { get; set; }

        [JsonPropertyName("name")]
        public NameDto Name { get; set; }

        [JsonPropertyName("revelation_place")]
        public RevelationPlaceDto RevelationPlace { get; set; }

        [JsonPropertyName("verses_count")]
        public int VersesCount { get; set; }

        [JsonPropertyName("words_count")]
        public int WordsCount { get; set; }

        [JsonPropertyName("letters_count")]
        public int LettersCount { get; set; }

        [JsonPropertyName("verses")]
        public List<VerseDto> Verses { get; set; }

        [JsonPropertyName("audio")]
        public List<AudioDto> Audio { get; set; }


    }
}
