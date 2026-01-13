using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quran.Infrastructure.Abstract
{
    public interface IVersesRepo
    {
        Task<Verse> Get(int id);
        Task<Verse> GetVerseBySurahName(int id, int surahId);
        // full text search
        Task<List<Verse>> SearchVersesAsync(string searchText);
        // like search
        Task<List<Verse>> SearchVersesLikeAsync(string searchText);
        // pagination search with like
        Task<List<Verse>> SearchVersesPaginationAsync(string searchText, int pageNumber, int pageSize);
        Task<int> CountSearch(string searchText);
        Task<Verse> StopVerse(int verseId);
        Task<Verse?> CheckStopVerse();
        // save changes
        Task<int> SaveChangesAsync();
    }
}
