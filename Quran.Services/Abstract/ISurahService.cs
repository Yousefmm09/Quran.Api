using Quran.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quran.Services.Abstract
{
    public interface ISurahService
    {
        Task<ApiResponse<List<SurahDto>>> GetAllSurahsAsync();
        Task<ApiResponse<SurahDto?>> GetSurahByIdAsync(int id);
        Task<ApiResponse<List<VersesDto>>> GetVersesBySurahId(int surahId);

    }
}
