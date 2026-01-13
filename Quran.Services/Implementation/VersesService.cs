using Quran.Infrastructure.Abstract;
using Quran.Infrastructure.Implementation;
using Quran.Infrastructure.Seeder;
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
        public VersesService(IVersesRepo versesRepository)
        {
            _versesRepository = versesRepository;
        }

        public async Task<ApiResponse<VersesDto>> Get(int id)
        {
            var verse = await _versesRepository.Get(id);
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
                Success: true,
                Message: "Verse retrieved successfully",
                Data: verseDto,
                Errors: null,
                TraceId: Guid.NewGuid().ToString()
            );
        }

        public async Task<ApiResponse<List<VersesDto>>> SearchVersesAsync(string searchText)
        {
            var verses= await _versesRepository.SearchVersesAsync(searchText);
            if (verses == null)
            {
                return new ApiResponse<List<VersesDto>>(
                    Success: false,
                    Message: "Verse not found",
                    Data: null,
                    Errors: new[] { "Verse not found" },
                    TraceId: Guid.NewGuid().ToString()
                );
            }
            var versesDto = verses.Select(x => new VersesDto
            {
                Id = x.Id,
                Number = x.Number,
                TextAr = x.TextAr
            }).ToList();
            return new ApiResponse<List<VersesDto>>(
                Success:true,
                Message:"Verses retrived success",
                Data: versesDto,
                Errors: null,
                TraceId: Guid.NewGuid().ToString()
                );
        }

        public async Task<ApiResponse<List<VersesDto>>> SearchVersesLikeAsync(string searchText)
        {
            var verses= await _versesRepository.SearchVersesLikeAsync(searchText);
            var versesDto = verses.Select(x => new VersesDto
            {
                Id = x.Id,
                Number = x.Number,
                TextAr = x.TextAr
            }).ToList();
            return new ApiResponse<List<VersesDto>>
                (
                Success:verses !=null ? true : false,
                Message: verses !=null ? "The verses retrived success" :"not found",
                Data: versesDto,
               Errors: verses == null ? new[] { "Verse not found" } : null,
                TraceId: Guid.NewGuid().ToString()
                );
        }

        public async Task<ApiResponse<VersesPaginationDto>> SearchVersesPaginationAsync(string searchText, int pageNumber, int pageSize)
        {
            
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
            // 1️⃣ get current stopped verse
            var currentStopped = await _versesRepository.CheckStopVerse();

            // 2️⃣ get target verse
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

            // 3️⃣ same verse already stopped
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

            // 4️⃣ unstop old one
            if (currentStopped != null)
            {
                currentStopped.StopVerse = false;
            }

            // 5️⃣ stop new one
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
