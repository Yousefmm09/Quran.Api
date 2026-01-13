using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Quran.Data.Entities;
using Quran.Infrastructure.Abstract;
using Quran.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quran.Infrastructure.Implementation
{
    public class AudioRepo:IAudioRepo
    {
        private readonly AppDb _appDb;
        public AudioRepo(AppDb appDb)
        {
         _appDb = appDb;   
        }

        public async Task<AudioRecitation> GetAudioRecitation(int id)
        {
            var audio = await _appDb.AudioRecitations.AsNoTracking().Include(x => x.Surah)
                .Where(x => x.Id == id)
                .Select(x => new AudioRecitation
                {
                    Id = x.Id,
                    ReciterNameAr = x.ReciterNameAr,
                    RewayaAr = x.RewayaAr,
                    Server = x.Server,
                    Link=x.Link,
                     Surah= new Surah
                    {
                         NameAr=x.Surah.NameAr
                    }
                }).FirstOrDefaultAsync();
            return audio;
        }

        public async Task<List<AudioRecitation>> GetAudioRecitationsAsync()
        {
            return await _appDb.AudioRecitations
                .AsNoTracking()
                .Include(x => x.Surah)
                .OrderBy(x => x.Id)
                .ToListAsync();
        }

        public async Task<string> GetSurahName(int surahId)
        {
            var surahName = _appDb.Surah.Where(x => x.Id == surahId)
                .Select(x => x.NameAr).FirstOrDefault();
            return surahName;
        }

        public async Task<List<AudioRecitation>> SearchRecitationandSurahName(string recitation, int surahId)
        {
            var sql = """
                     SELECT 
                       Id,
                       ReciterNameAr,
                        SurahId,
                         RewayaAr,
                         Link,
                         Server
                     FROM AudioRecitations
                    WHERE 
                     SurahId = @surahId
                      AND CONTAINS(ReciterNameAr, @reciter)
                    """;

            var reciterParam = $"\"{recitation}\"";

            return await _appDb.AudioRecitations
                .FromSqlRaw(sql,
                    new SqlParameter("@surahId", surahId),
                    new SqlParameter("@reciter", reciterParam))
                .AsNoTracking()
                .Select(x => new AudioRecitation
                {
                    Id = x.Id,
                    ReciterNameAr = x.ReciterNameAr,
                    SurahId = x.SurahId,
                    RewayaAr = x.RewayaAr,
                    Link = x.Link,
                    Server = x.Server
                })
                .ToListAsync();
        }
    }
}
