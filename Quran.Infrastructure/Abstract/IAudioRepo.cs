using Quran.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quran.Infrastructure.Abstract
{
    public interface IAudioRepo
    {
        Task<List<AudioRecitation>> GetAudioRecitationsAsync();
        Task<AudioRecitation> GetAudioRecitation(int Id);
        Task<string> GetSurahName(int surahId);
        Task<List<AudioRecitation>> SearchRecitationandSurahName(string recitation, int surahId);
    }
}
