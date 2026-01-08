using Quran.Data.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Surah
{
        public int Id { get; set; }
        public int Number { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string Transliteration { get; set; }
        public string RevelationPlaceAr { get; set; }
        public string RevelationPlaceEn { get; set; }
        public int VersesCount { get; set; }
        public int WordsCount { get; set; }
        public int LettersCount { get; set; }

        // Navigation Properties
        public ICollection<Verse> Verses { get; set; }
        public ICollection<AudioRecitation> AudioRecitations { get; set; }
}
