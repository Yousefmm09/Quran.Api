using Quran.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quran.Services.Abstract
{
    public interface IVerses
    {
        Task<ApiResponse<VersesDto>> Get(int id);
        Task<ApiResponse<VersesDto>> GetVerseBySurahName(int id, int surahId);
        Task<ApiResponse<List<VersesDto>>> SearchVersesAsync(string searchText);
        Task<ApiResponse<List<VersesDto>>> SearchVersesLikeAsync(string searchText);
        Task<ApiResponse<VersesPaginationDto>> SearchVersesPaginationAsync(string searchText, int pageNumber, int pageSize);
        Task<ApiResponse<StopVerseDto>> StopVerse(int verseId);

    }
}
