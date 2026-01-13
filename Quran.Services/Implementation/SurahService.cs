using Microsoft.Extensions.Caching.Memory;
using Quran.Infrastructure.Abstract;
using Quran.Services.Abstract;
using Quran.Services.Dto;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quran.Services.Implementation
{
    public class SurahService : ISurahService
    {
        private readonly ISurahRepo _surah;
        private readonly IMemoryCache _memory;
        public SurahService(ISurahRepo surah, IMemoryCache memory)
        {
            _surah = surah;
            _memory = memory;
        }
        public async Task<ApiResponse<List<SurahDto>>> GetAllSurahsAsync()
        {
            const string cacheKey = "surahs:all:v1";

            if (_memory.TryGetValue(cacheKey, out List<SurahDto> cachedSurahs))
            {
                Log.Information("Surahs retrieved from cache: {Count}", cachedSurahs.Count);

                return new ApiResponse<List<SurahDto>>(
                    Success: true,
                    Message: "Surahs retrieved successfully from cache",
                    Data: cachedSurahs,
                    Errors: null,
                    TraceId: Guid.NewGuid().ToString()
                );
            }

            var surahs = await _surah.GetAllSurahsAsync();

            Log.Information("Surahs retrieved from database: {Count}", surahs.Count());

            var surahDtos = surahs.Select(s => new SurahDto
            {
                Id = s.Id,
                Name = s.NameAr,
                NumberOfSurah = s.Number,
                RevelationType = s.RevelationPlaceAr,
                VerserCount = s.VersesCount,
            }).ToList();

            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                SlidingExpiration = TimeSpan.FromMinutes(5),
                Priority = CacheItemPriority.High
            };

            _memory.Set(cacheKey, surahDtos, options);

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
            var cacheKey = $"surah_{id}";
            if(_memory.TryGetValue(cacheKey, out SurahDto? cachedSurah))
            {
                var surah = await _surah.GetSurahByIdAsync(id);
                Log.Information("Retrieved surah with ID {Id}: {Surah}", id, surah != null ? surah.NameEn : "Not Found");
                var option = new MemoryCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                    Priority = CacheItemPriority.High,
                    SlidingExpiration = TimeSpan.FromMinutes(5)

                };
                _memory.Set(cacheKey, cachedSurah, option);
            }
            return new ApiResponse<SurahDto?>(
                Success: cachedSurah != null,
                Message: cachedSurah != null ? "Surah retrieved successfully" : "Surah not found",
                Data: cachedSurah,
                Errors: null,
                TraceId: Guid.NewGuid().ToString()
            );
        }

        public async Task<ApiResponse<List<VersesDto>>> GetVersesBySurahId(int surahId)
        {
            var cacheKey = $"verses_surah_{surahId}";
            if (_memory.TryGetValue(cacheKey, out List<VersesDto> cachedVerses))
            {
                var verse = await _surah.GetVersesBySurahId(surahId);
                var surahName = _surah.GetSurahNameAsync(surahId);
                var verses = verse.Select(x => new VersesDto
                {
                    Id = x.Id,
                    Number = x.Number,
                    VersesNumber = x.Number,
                    TextAr = x.TextAr,
                    TextArabicSearch = x.TextArabicSearch
                }).ToList();
                var option = new MemoryCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                    Priority = CacheItemPriority.High,
                    SlidingExpiration = TimeSpan.FromMinutes(5)
                };
                _memory.Set(cacheKey, verses, option);
            }
            return new ApiResponse<List<VersesDto>>(
                Success: cachedVerses != null,
                Message: cachedVerses != null ? $"The verser retrived successfully ,the name of surah is { _surah.GetSurahNameAsync(surahId)}" : "not found verses",
                Data: cachedVerses,
                Errors: null,
                TraceId: Guid.NewGuid().ToString()
                );
        }

    }
}
