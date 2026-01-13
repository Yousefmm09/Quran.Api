using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Quran.Infrastructure.Context;
using System.Text.Json;

namespace Infrastructure.Data
{
    public class QuranSeeder
    {
        private readonly AppDb _context;
        private readonly ILogger<QuranSeeder> _logger;

        public QuranSeeder(AppDb context, ILogger<QuranSeeder> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// إزالة التشكيل والحركات من النص العربي
        /// </summary>
        private static string RemoveArabicDiacritics(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            // Arabic diacritics Unicode ranges
            char[] arabicDiacritics = new char[]
            {
                '\u064B', // Fathatan
                '\u064C', // Dammatan
                '\u064D', // Kasratan
                '\u064E', // Fatha
                '\u064F', // Damma
                '\u0650', // Kasra
                '\u0651', // Shadda
                '\u0652', // Sukun
                '\u0653', // Maddah
                '\u0654', // Hamza Above
                '\u0655', // Hamza Below
                '\u0656', // Subscript Alef
                '\u0657', // Inverted Damma
                '\u0658', // Mark Noon Ghunna
                '\u0670', // Superscript Alef
            };

            foreach (var diacritic in arabicDiacritics)
            {
                text = text.Replace(diacritic.ToString(), "");
            }

            return text.Trim();
        }

        public async Task SeedTextArabicSearchFromJson(string jsonFilePath)
        {
            try
            {
                _logger.LogInformation("Starting to seed TextArabicSearch from JSON...");

                // 1. قراءة الملف
                if (!File.Exists(jsonFilePath))
                {
                    _logger.LogError($"JSON file not found: {jsonFilePath}");
                    return;
                }

                string jsonContent = await File.ReadAllTextAsync(jsonFilePath);

                // 2. Parse JSON
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var quranData = JsonSerializer.Deserialize<Dictionary<string, List<VerseJson>>>(jsonContent, options);

                if (quranData == null || !quranData.Any())
                {
                    _logger.LogError("JSON file is empty or invalid");
                    return;
                }

                // 3. حساب عدد الآيات
                int totalVerses = quranData.Sum(x => x.Value.Count);
                _logger.LogInformation($"Found {totalVerses} verses in JSON");

                // 4. معالجة كل سورة - تحسين الأداء بتحميل كل السورة مرة واحدة
                int processedCount = 0;
                int updatedCount = 0;

                foreach (var surah in quranData.OrderBy(x => int.Parse(x.Key)))
                {
                    int surahNumber = int.Parse(surah.Key);
                    var versesJson = surah.Value.OrderBy(v => v.Verse).ToList();

                    _logger.LogInformation($"Processing Surah {surahNumber} with {versesJson.Count} verses...");

                    // تحميل كل آيات السورة مرة واحدة
                    var versesInDb = await _context.Verses
                        .Where(v => v.SurahId == surahNumber)
                        .ToListAsync();

                    foreach (var verseJson in versesJson)
                    {
                        // البحث عن الآية في القائمة المحملة
                        var verseInDb = versesInDb.FirstOrDefault(v => v.Number == verseJson.Verse);
                        
                        if (verseInDb == null)
                        {
                            _logger.LogWarning(
                                $"Verse not found in DB: Surah {surahNumber}, Verse {verseJson.Verse}");
                            processedCount++;
                            continue;
                        }

                        // تحديث TextArabicSearch - إزالة التشكيل
                        if (!string.IsNullOrEmpty(verseJson.Text))
                        {
                            verseInDb.TextArabicSearch = RemoveArabicDiacritics(verseJson.Text);
                            updatedCount++;
                        }

                        processedCount++;
                    }
                    
                    // حفظ بعد كل سورة لتجنب مشاكل الأداء
                    await _context.SaveChangesAsync();
                    _logger.LogInformation($"Progress: {processedCount}/{totalVerses} verses processed...");
                }

                // حفظ نهائي
                await _context.SaveChangesAsync();

                _logger.LogInformation($"✅ Seeding completed!");
                _logger.LogInformation($"Processed: {processedCount} verses");
                _logger.LogInformation($"Updated: {updatedCount} verses");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during seeding");
                throw;
            }
        }
    }

    // Model للـ JSON
    public class VerseJson
    {
        [System.Text.Json.Serialization.JsonPropertyName("chapter")]
        public int Chapter { get; set; }
        
        [System.Text.Json.Serialization.JsonPropertyName("verse")]
        public int Verse { get; set; }
        
        [System.Text.Json.Serialization.JsonPropertyName("text")]
        public string? Text { get; set; }
    }
}