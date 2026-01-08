using Quran.Data.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Verse
{
    public int Id { get; set; }
    public int Number { get; set; }
    public string TextAr { get; set; }
    public string TextEn { get; set; }
    public int Juz { get; set; }
    public int Page { get; set; }

    // Sajda properties
    public bool HasSajda { get; set; }
    public int? SajdaId { get; set; }
    public bool? SajdaRecommended { get; set; }
    public bool? SajdaObligatory { get; set; }

    public int SurahId { get; set; }
    public Surah Surah { get; set; }
}