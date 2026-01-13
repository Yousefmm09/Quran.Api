using Microsoft.Extensions.Caching.Memory;
using Quran.Infrastructure.Abstract;
using Quran.Infrastructure.Implementation;
using Quran.Services.Abstract;
using Quran.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quran.Services.Implementation
{
    public class VersesService:IVerses
    {
        private readonly IVersesRepo _versesRepository;
        private readonly IMemoryCache _memory;
        public VersesService(IVersesRepo versesRepository, IMemoryCache memory)
        {
            _versesRepository = versesRepository;
            _memory = memory;
        }

        public async Task<ApiResponse<VersesDto>> Get(int id)
        {
            var cacheKey = $"Verse_{id}";
            if (_memory.TryGetValue(cacheKey, out ApiResponse<VersesDto> cachedResponse))
            {
                return cachedResponse;
            }
            else
            {
                var verse = await _versesRepository.Get(id);
                if (verse == null)
                {
                    return new ApiResponse<VersesDto>(
                        Success: false,
                        Message: "Verse not found",
                        Data: null,
                        Errors: new[] { "Verse not found" },
                        TraceId: Guid.NewGuid().ToString()
                    );
                }
                var verseDto = new VersesDto
                {
                    Id = verse.Id,
                    Number = verse.Number,
                    TextAr = verse.TextAr,
                    TextArabicSearch = verse.TextArabicSearch,
                };
                var response = new ApiResponse<VersesDto>(
                    Success: true,
                    Message: "Verse retrieved successfully",
                    Data: verseDto,
                    Errors: null,
                    TraceId: Guid.NewGuid().ToString()
                );
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(30));
                _memory.Set(cacheKey, response, cacheEntryOptions);
                return response;
            }
            
        }

        public async Task<ApiResponse<VersesDto>> GetVerseBySurahName(int id, int surahId)
        {
            var verse = await _versesRepository.GetVerseBySurahName(id, surahId);

            if (verse == null)
            {
                return new ApiResponse<VersesDto>(
                    Success: false,
                    Message: "Verse not found",
                    Data: null,
                    Errors: new[] { "Verse not found" },
                    TraceId: Guid.NewGuid().ToString()
                );
            }

            var verseDto = new VersesDto
            {
                Id = verse.Id,
                Number = verse.Number,
                TextAr = verse.TextAr,
                TextArabicSearch = verse.TextArabicSearch,
            };
            return new ApiResponse<VersesDto>(
                Success: verseDto != null,
                Message: verseDto != null ? "Verse retrieved successfully" : "not found",
                Data: verseDto,
                Errors: verseDto == null ? new[] { "Verse not found" } : null,
                TraceId: Guid.NewGuid().ToString()
                );
        }

        public async Task<ApiResponse<List<VersesDto>>> SearchVersesAsync(string searchText)
        {
            var cacheKey = $"SearchVerses_{searchText}";
            if (_memory.TryGetValue(cacheKey, out ApiResponse<List<VersesDto>> cachedResponse))
            {
                return cachedResponse;
            }

            var verses = await _versesRepository.SearchVersesAsync(searchText);
           var versesDto = verses.Select(x => new VersesDto
            {
                Id = x.Id,
                Number = x.Number,
                TextAr = x.TextAr
            }).ToList();
            var response = new ApiResponse<List<VersesDto>>(
                Success: verses != null && verses.Any(),
                Message: verses != null && verses.Any() ? "The verses retrieved successfully" : "not found",
                Data: versesDto,
                Errors: verses == null || !verses.Any() ? new[] { "Verse not found" } : null,
                TraceId: Guid.NewGuid().ToString()
            );
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(30));
            _memory.Set(cacheKey, response, cacheEntryOptions);
            return response;
        }

        public async Task<ApiResponse<List<VersesDto>>> SearchVersesLikeAsync(string searchText)
        {
            var cacheKey = $"SearchVersesLike_{searchText}";
            if (_memory.TryGetValue(cacheKey, out ApiResponse<List<VersesDto>> cachedResponse))
            {
                return cachedResponse;
            }
            var verses = await _versesRepository.SearchVersesLikeAsync(searchText);
            var versesDto = verses.Select(x => new VersesDto
            {
                Id = x.Id,
                Number = x.Number,
                TextAr = x.TextAr
            }).ToList();
            var response = new ApiResponse<List<VersesDto>>(
                Success: verses != null && verses.Any(),
                Message: verses != null && verses.Any() ? "The verses retrieved successfully" : "not found",
                Data: versesDto,
                Errors: verses == null || !verses.Any() ? new[] { "Verse not found" } : null,
                TraceId: Guid.NewGuid().ToString()
            );
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(30));
            _memory.Set(cacheKey, response, cacheEntryOptions);
            return response;
        }

        public async Task<ApiResponse<VersesPaginationDto>> SearchVersesPaginationAsync(string searchText, int pageNumber, int pageSize)
        {
            var cacheKey = $"SearchVersesPagination_{searchText}_{pageNumber}_{pageSize}";
            if (_memory.TryGetValue(cacheKey, out ApiResponse<VersesPaginationDto> cachedResponse))
            {
                return cachedResponse;
            }

            var versesPage = await _versesRepository.SearchVersesPaginationAsync(searchText, pageNumber, pageSize);

            var totalCount = await _versesRepository.CountSearch(searchText);

            var versesDto = versesPage.Select(v => new VersesDto
            {
                Id = v.Id,
                Number = v.Number,
                TextAr = v.TextAr
            }).ToList();

            var paginationDto = new VersesPaginationDto
            {
                Verses = versesDto,
                TotalCount = totalCount
            };

            _memory.Set(cacheKey, new ApiResponse<VersesPaginationDto>(
                Success: versesPage.Any(),
                Message: versesPage.Any() ? "The verses retrieved successfully" : "No verses found",
                Data: paginationDto,
                Errors: versesPage.Any() ? null : new[] { "Verse not found" },
                TraceId: Guid.NewGuid().ToString()
            ), new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            });
            return new ApiResponse<VersesPaginationDto>(
                Success: versesPage.Any(),
                Message: versesPage.Any() ? "The verses retrieved successfully" : "No verses found",
                Data: paginationDto,
                Errors: versesPage.Any() ? null : new[] { "Verse not found" },
                TraceId: Guid.NewGuid().ToString()
            );

        }

        public async Task<ApiResponse<StopVerseDto>> StopVerse(int verseId)
        {
           
            var currentStopped = await _versesRepository.CheckStopVerse();

            var verse = await _versesRepository.Get(verseId);
            if (verse == null)
            {
                return new ApiResponse<StopVerseDto>(
                    Success: false,
                    Message: "Verse not found",
                    Data: null,
                    Errors: new[] { "Verse not found" },
                    TraceId: Guid.NewGuid().ToString()
                );
            }

            if (currentStopped != null && currentStopped.Id == verseId)
            {
                return new ApiResponse<StopVerseDto>(
                    Success: false,
                    Message: "Verse is already stopped",
                    Data: null,
                    Errors: new[] { "Verse is already stopped" },
                    TraceId: Guid.NewGuid().ToString()
                );
            }

            if (currentStopped != null)
            {
                currentStopped.StopVerse = false;
            }

            verse.StopVerse = true;

            await _versesRepository.SaveChangesAsync();

            return new ApiResponse<StopVerseDto>(
                Success: true,
                Message: "Verse stopped successfully",
                Data: new StopVerseDto
                {
                    ID = verse.Id,
                    TextAr = verse.TextAr ?? string.Empty
                },
                Errors: null,
                TraceId: Guid.NewGuid().ToString()
            );
        }




    }
}
