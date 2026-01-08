// Data/QuranDataSeeder.cs
using Microsoft.EntityFrameworkCore;
using Quran.Data.Entities;
using Quran.Infrastructure.Context;
using Quran.Infrastructure.Seeder;
using System.Text.Json;

public class QuranDataSeeder
{
    private readonly AppDb _context;
    private readonly string _surahFolderPath;
    private readonly List<string> _errorDetails = new();

    public QuranDataSeeder(AppDb context, string surahFolderPath)
    {
        _context = context;
        _surahFolderPath = surahFolderPath;
    }

    public async Task SeedAsync()
    {
        if (await _context.Surah.AnyAsync())
        {
            Console.WriteLine("Database already seeded. Skipping...");
            return;
        }

        Console.WriteLine("Starting Quran data seeding...");
        Console.WriteLine($"Reading from folder: {_surahFolderPath}");

        try
        {
            var jsonFiles = Directory.GetFiles(_surahFolderPath, "surah_*.json")
                                     .OrderBy(f => GetSurahNumber(f))
                                     .ToList();

            if (!jsonFiles.Any())
            {
                Console.WriteLine("No surah JSON files found!");
                return;
            }

            Console.WriteLine($"Found {jsonFiles.Count} surah files to process.");
            Console.WriteLine(new string('-', 80));

            int successCount = 0;
            int errorCount = 0;

            foreach (var jsonFile in jsonFiles)
            {
                var fileName = Path.GetFileName(jsonFile);

                try
                {
                    var jsonContent = await File.ReadAllTextAsync(jsonFile);

                    // Try to deserialize
                    var surahDto = JsonSerializer.Deserialize<SurahDto>(jsonContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        AllowTrailingCommas = true,
                        ReadCommentHandling = JsonCommentHandling.Skip
                    });

                    if (surahDto == null)
                    {
                        errorCount++;
                        var error = $"{fileName}: Deserialization returned null";
                        _errorDetails.Add(error);
                        Console.WriteLine($"✗ [{errorCount:000}] {error}");
                        continue;
                    }

                    // Validate required fields
                    if (string.IsNullOrEmpty(surahDto.Name?.Ar) || string.IsNullOrEmpty(surahDto.Name?.En))
                    {
                        errorCount++;
                        var error = $"{fileName}: Missing required name fields";
                        _errorDetails.Add(error);
                        Console.WriteLine($"✗ [{errorCount:000}] {error}");
                        continue;
                    }

                    var surah = MapToSurah(surahDto);
                    _context.Surah.Add(surah);

                    successCount++;
                    Console.WriteLine($"✓ [{successCount:000}] {fileName,-20} | Surah {surah.Number:000}: {surah.NameEn,-25} | V:{surah.VersesCount:000} | A:{surah.AudioRecitations.Count:000}");
                }
                catch (JsonException jsonEx)
                {
                    errorCount++;
                    var error = $"{fileName}: JSON Error at {jsonEx.Path} - {jsonEx.Message}";
                    _errorDetails.Add(error);
                    Console.WriteLine($"✗ [{errorCount:000}] {error}");
                }
                catch (Exception ex)
                {
                    errorCount++;
                    var error = $"{fileName}: {ex.GetType().Name} - {ex.Message}";
                    _errorDetails.Add(error);
                    Console.WriteLine($"✗ [{errorCount:000}] {error}");
                }
            }

            Console.WriteLine(new string('-', 80));

            if (successCount > 0)
            {
                Console.WriteLine("Saving to database...");
                await _context.SaveChangesAsync();
            }

            Console.WriteLine(new string('=', 80));
            Console.WriteLine("SEEDING SUMMARY");
            Console.WriteLine(new string('=', 80));
            Console.WriteLine($"✓ Success: {successCount}/{jsonFiles.Count} surahs seeded");

            if (successCount > 0)
            {
                Console.WriteLine($"  • Total verses: {await _context.Verses.CountAsync()}");
                Console.WriteLine($"  • Total audio recitations: {await _context.AudioRecitations.CountAsync()}");
            }

            if (errorCount > 0)
            {
                Console.WriteLine($"\n⚠ Errors: {errorCount} files failed");
                Console.WriteLine(new string('-', 80));
                Console.WriteLine("ERROR DETAILS:");
                foreach (var error in _errorDetails)
                {
                    Console.WriteLine($"  • {error}");
                }
            }

            Console.WriteLine(new string('=', 80));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✗ CRITICAL ERROR: {ex.Message}");
            Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            throw;
        }
    }

    private int GetSurahNumber(string filePath)
    {
        var fileName = Path.GetFileNameWithoutExtension(filePath);
        var numberPart = fileName.Replace("surah_", "");
        return int.TryParse(numberPart, out int number) ? number : 0;
    }

    // Data/QuranDataSeeder.cs
    private Surah MapToSurah(SurahDto dto)
    {
        var surah = new Surah
        {
            Number = dto.Number,
            NameAr = dto.Name?.Ar ?? "Unknown",
            NameEn = dto.Name?.En ?? "Unknown",
            Transliteration = dto.Name?.Transliteration ?? "",
            RevelationPlaceAr = dto.RevelationPlace?.Ar ?? "",
            RevelationPlaceEn = dto.RevelationPlace?.En ?? "",
            VersesCount = dto.VersesCount,
            WordsCount = dto.WordsCount,
            LettersCount = dto.LettersCount,
            Verses = new List<Verse>(),
            AudioRecitations = new List<AudioRecitation>()
        };

        if (dto.Verses != null && dto.Verses.Any())
        {
            foreach (var verseDto in dto.Verses)
            {
                if (verseDto?.Text != null)
                {
                    var sajda = verseDto.Sajda;

                    surah.Verses.Add(new Verse
                    {
                        Number = verseDto.Number,
                        TextAr = verseDto.Text.Ar ?? "",
                        TextEn = verseDto.Text.En ?? "",
                        Juz = verseDto.Juz,
                        Page = verseDto.Page,
                        HasSajda = sajda?.HasSajda ?? false,
                        SajdaId = sajda?.Id,
                        SajdaRecommended = sajda?.Recommended,
                        SajdaObligatory = sajda?.Obligatory
                    });
                }
            }
        }

        // Audio mapping remains the same
        if (dto.Audio != null && dto.Audio.Any())
        {
            foreach (var audioDto in dto.Audio)
            {
                if (audioDto?.Reciter != null && audioDto?.Rewaya != null)
                {
                    surah.AudioRecitations.Add(new AudioRecitation
                    {
                        ReciterId = audioDto.Id,
                        ReciterNameAr = audioDto.Reciter.Ar ?? "",
                        ReciterNameEn = audioDto.Reciter.En ?? "",
                        RewayaAr = audioDto.Rewaya.Ar ?? "",
                        RewayaEn = audioDto.Rewaya.En ?? "",
                        Server = audioDto.Server ?? "",
                        Link = audioDto.Link ?? ""
                    });
                }
            }
        }

        return surah;
    }
}