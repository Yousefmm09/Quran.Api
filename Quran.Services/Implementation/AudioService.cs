using Microsoft.Extensions.Caching.Memory;
using Quran.Data.Entities;
using Quran.Infrastructure.Abstract;
using Quran.Infrastructure.Context;
using Quran.Services.Abstract;
using Quran.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quran.Services.Implementation
{
    public class AudioService:IAudioService
    {
        private readonly IMemoryCache _memory;
        private readonly IAudioRepo _audioRepo;
        public AudioService(IAudioRepo audioRepo, IMemoryCache memory)
        {
           _audioRepo = audioRepo;
           _memory = memory;
        }

        public async Task<ApiResponse<AudioRecitationDto>> GetAudioRecitation(int Id)
        {
           var cacheKey = $"AudioRecitation-{Id}";
           if (!_memory.TryGetValue(cacheKey, out AudioRecitationDto audioDto))
           {
               var audio= await _audioRepo.GetAudioRecitation(Id);
               audioDto = new AudioRecitationDto()
               {
                   Id=audio.Id,
                   ReciterNameAr=audio.ReciterNameAr,
                   RewayaAr=audio.RewayaAr,
                   SurahName=audio.Surah.NameAr,
                   Server=audio.Server,
               };
                var option = new MemoryCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                    Priority = CacheItemPriority.High,
                    SlidingExpiration = TimeSpan.FromMinutes(5)

                };
                _memory.Set(cacheKey, audioDto, option);
            }
           return new ApiResponse<AudioRecitationDto>
                (
                Success: audioDto !=null ? true : false,
                Message: audioDto !=null ? "The audio retrived successfully " : "not found audio",
                Data:audioDto,
                 Errors: audioDto == null ? new[] { "Audio not found" } : null,
                TraceId: Guid.NewGuid().ToString()
                );
        }

        public async Task<ApiResponse<List<AudioRecitationDto>>> GetAudioRecitationsAsync( int pageNumber ,int pageSize)
        {
            var cacheKey = $"AudioRecitations-Page{pageNumber}-Size{pageSize}";
            if (!_memory.TryGetValue(cacheKey, out List<AudioRecitationDto> audioDtos))
            {
                var audios = await _audioRepo.GetAudioRecitationsAsync();
                audioDtos = audios
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(audio => new AudioRecitationDto
                    {
                        Id = audio.Id,
                        ReciterNameAr = audio.ReciterNameAr,
                        RewayaAr = audio.RewayaAr,
                        SurahName = audio.Surah.NameAr,
                        Server = audio.Server,
                        Link = audio.Link
                    })
                    .ToList();
                var option = new MemoryCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                    Priority = CacheItemPriority.High,
                    SlidingExpiration = TimeSpan.FromMinutes(5)

                };
                _memory.Set(cacheKey, audioDtos, option);
              
            }
            return new ApiResponse<List<AudioRecitationDto>>
                (
                Success: audioDtos != null && audioDtos.Any() ? true : false,
                Message: audioDtos != null && audioDtos.Any() ? "Audio recitations retrieved successfully" : "No audio recitations found",
                Data: audioDtos,
                Errors: audioDtos == null || !audioDtos.Any() ? new[] { "No audio recitations available" } : null,
                TraceId: Guid.NewGuid().ToString()
                );
            
        }

        public async Task<ApiResponse<List<SearchAudioRectiterbySurahIdDto>>> SearchRecitationandSurahName(string recitation, int surahId)
        {
            var audios = await  _audioRepo.SearchRecitationandSurahName(recitation, surahId);
            var audioDto= audios.Select(audio => new SearchAudioRectiterbySurahIdDto
            {
                Id = audio.Id,
                ReciterNameAr = audio.ReciterNameAr,
                RewayaAr = audio.RewayaAr,
                SurahId= audio.SurahId,
                Server = audio.Server,
                Link = audio.Link
            }).ToList();
            return new ApiResponse<List<SearchAudioRectiterbySurahIdDto>>
                (
                Success: audioDto != null && audioDto.Any() ? true : false,
                Message: audioDto != null && audioDto.Any() ? "Audio recitations retrieved successfully" : "No audio recitations found",
                Data: audioDto,
                Errors: audioDto == null || !audioDto.Any() ? new[] { "No audio recitations available" } : null,
                TraceId: Guid.NewGuid().ToString()
                );
        }
    }
}
