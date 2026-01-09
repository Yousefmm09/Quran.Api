using Quran.Infrastructure.Abstract;
using Quran.Services.Abstract;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quran.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Quran.Services.Implementation
{
    public class SurahService : ISurahService
    {
        private readonly ISurahRepo _surah;

        public SurahService(ISurahRepo surah)
        {
         _surah = surah;   
        }
        public async Task<ApiResponse<List<SurahDto>>> GetAllSurahsAsync()
        {
            var surah = await _surah.GetAllSurahsAsync();
            Log.Information("Retrieved {Count} surahs", surah.Count());
            var surahDtos = surah.Select(s => new SurahDto
            {
                Id = s.Id,
                Name = s.NameAr,
                NumberOfSurah = s.Number,
                RevelationType = s.RevelationPlaceAr,
                VerserCount = s.VersesCount,
            }).ToList();
            return new ApiResponse<List<SurahDto>>(
                Success: true,
                Message: "Surahs retrieved successfully",
                Data: surahDtos,
                Errors: null,
                TraceId: Guid.NewGuid().ToString()
            );
        }

        public async Task<ApiResponse<SurahDto?>> GetSurahByIdAsync(int id)
        {
            var surah = await _surah.GetSurahByIdAsync(id);
            Log.Information("Retrieved surah with ID {Id}: {Surah}", id, surah != null ? surah.NameEn : "Not Found");
            return new ApiResponse<SurahDto?>(
                Success: surah != null,
                Message: surah != null ? "Surah retrieved successfully" : "Surah not found",
                Data: surah != null ? new SurahDto
                {
                    Id = surah.Id,
                    Name = surah.NameAr,
                    NumberOfSurah = surah.Number,
                    RevelationType = surah.RevelationPlaceAr,
                    VerserCount=surah.VersesCount,
                } : null,
                Errors: null,
                TraceId: Guid.NewGuid().ToString()
            );
        }

        public async Task<ApiResponse<List<VersesDto>>> GetVersesBySurahId(int surahId)
        {
            var verse = await _surah.GetVersesBySurahId(surahId);
            var surahName = _surah.GetSurahNameAsync(surahId);
            var verses = verse.Select(x => new VersesDto
            {
                Id= x.Id,
                VersesNumber=x.Number,
                TextAr = x.TextAr,

            }).ToList();
            Log.Information($"Verses: {surahId}");
            return new ApiResponse<List<VersesDto>>(
                Success: verses != null,
                Message: verses != null ? $"The verser retrived successfully ,the name of surah is {surahName}" : "not found verses",
                Data: verses,
                Errors: null,
                TraceId: Guid.NewGuid().ToString()
                );
        }

    }
}
