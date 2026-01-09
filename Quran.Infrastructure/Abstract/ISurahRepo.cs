using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quran.Infrastructure.Abstract
{
    public interface ISurahRepo
    {
        Task<Surah?> GetSurahByIdAsync(int id);
        Task<List<Surah>> GetAllSurahsAsync();
        Task<List<Verse>>GetVersesBySurahId(int surahId);
        string GetSurahNameAsync(int id);

    }
}
