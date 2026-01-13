using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Quran.Infrastructure.Abstract;
using Quran.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quran.Infrastructure.Implementation
{
    public class Verses : IVersesRepo
    {
        private readonly AppDb _appDb;
        public Verses(AppDb appDb)
        {
            _appDb = appDb;
        }

        public async Task<Verse> Get(int id)
        {
            var verses = await _appDb.Verses.FirstOrDefaultAsync(x => x.Id == id);
            return verses;
        }

        public async Task<Verse> GetVerseBySurahName(int id, int surahId)
        {
            var verse = await _appDb.Verses
                .Where(x => x.Number == id && x.SurahId == surahId)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return verse;
        }

        public async Task<List<Verse>> SearchVersesAsync(string searchText)
        {
            var verse = await _appDb.Verses.Where(v => EF.Functions.FreeText(v.TextArabicSearch, searchText))
                .AsNoTracking()
                .Select(x => new Verse
                {
                    TextAr = x.TextAr,
                    Number = x.Number,
                })
                .ToListAsync();
            return verse;
        }

        public Task<List<Verse>> SearchVersesLikeAsync(string searchText)
        {
            var verse = _appDb.Verses.Where(v => EF.Functions.Like(v.TextArabicSearch, $"%{searchText}%"))
                .AsNoTracking()
                .OrderBy(x => x.Id)
                .Select(x => new Verse
                {
                    TextAr = x.TextAr,
                    Number = x.Number,
                })
                .ToListAsync();
            return verse;
        }

        public Task<List<Verse>> SearchVersesPaginationAsync(string searchText, int pageNumber, int pageSize)
        {
            var verse = _appDb.Verses.Where(v => EF.Functions.Like(v.TextArabicSearch, $"%{searchText}%"))
                .AsNoTracking()
                .OrderBy(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new Verse
                {
                    TextAr = x.TextAr,
                    Number = x.Number,
                })
                .ToListAsync();
            return verse;
        }
        //count pagination
        public async Task<int> CountSearch(string searchText)
        {
            var count = await _appDb.Verses.Where(v => EF.Functions.Like(v.TextArabicSearch, $"%{searchText}%"))
                .AsNoTracking()
                .CountAsync();
            return count;
        }

        public async Task<Verse> StopVerse(int verseId)
        {
            var verse = await _appDb.Verses.FirstOrDefaultAsync(x => x.Id == verseId);
            return verse;
        }
        public async Task<Verse?> CheckStopVerse()
        {
             var verse=await _appDb.Verses.FirstOrDefaultAsync(x => x.StopVerse);
                return verse;
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _appDb.SaveChangesAsync();
        }
    }
}
