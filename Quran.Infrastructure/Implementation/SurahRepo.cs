using Microsoft.EntityFrameworkCore;
using Quran.Infrastructure.Abstract;
using Quran.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quran.Infrastructure.Implementation
{
    public class SurahRepo:ISurahRepo
    {
        private readonly AppDb _appDb;
        public SurahRepo(AppDb appDb)
        {
            _appDb=appDb;
        }

        public async Task<List<Surah>> GetAllSurahsAsync()
        {
            return await _appDb.Surah
                .AsNoTracking()
                .Select(x => new Surah
                {
                    Id = x.Id,
                    Number = x.Number,
                    NameAr = x.NameAr,
                    RevelationPlaceAr = x.RevelationPlaceAr,
                    VersesCount = x.VersesCount
                })
                .ToListAsync();
        }

        public Task<Surah?> GetSurahByIdAsync(int id)
        {
            var surah = _appDb.Surah.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return surah;
        }
        public string GetSurahNameAsync(int id)
        {
            var Sname=  _appDb.Surah.Find(id);
            return Sname.NameAr;
        }
        public async Task<List<Verse>> GetVersesBySurahId(int surahId)
        {
            var surah = await _appDb.Verses.Include(x => x.Surah).Where(x => x.SurahId == surahId)
                .Select(x => new Verse
                {
                    Id = x.Id,
                    TextAr=x.TextAr,
                    Number = x.Number
                }).ToListAsync();
            return surah;
        }
    }
}
