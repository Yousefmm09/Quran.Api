using Quran.Data.Entities;
using Quran.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quran.Services.Abstract
{
    public interface IAudioService
    {
        Task<ApiResponse<AudioRecitationDto>> GetAudioRecitation(int Id);
        Task<ApiResponse<List<AudioRecitationDto>>> GetAudioRecitationsAsync(int pageNumber, int pageSize);
        Task<ApiResponse<List<SearchAudioRectiterbySurahIdDto>>> SearchRecitationandSurahName(string recitation, int surahId);
    }
}
