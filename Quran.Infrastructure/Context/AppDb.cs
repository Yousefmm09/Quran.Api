using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Quran.Data.Entities.Identity;
using Quran.Data.Entities;

namespace Quran.Infrastructure.Context
{
    public class AppDb :DbContext
    {

        public AppDb(DbContextOptions<AppDb> options) : base(options)
        {
        }
       
         public DbSet<Surah> Surah { get; set; }
        public DbSet<Verse> Verses { get; set; }
        public DbSet<AudioRecitation> AudioRecitations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Surah Configuration
            modelBuilder.Entity<Surah>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Number).IsRequired();
                entity.Property(e => e.NameAr).IsRequired().HasMaxLength(100);
                entity.Property(e => e.NameEn).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Transliteration).HasMaxLength(100);

                entity.HasMany(e => e.Verses)
                      .WithOne(e => e.Surah)
                      .HasForeignKey(e => e.SurahId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.AudioRecitations)
                      .WithOne(e => e.Surah)
                      .HasForeignKey(e => e.SurahId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Verse Configuration
            modelBuilder.Entity<Verse>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.TextAr).IsRequired();
                entity.Property(e => e.TextArabicSearch).HasMaxLength(2000);
                entity.Property(e => e.TextEn).IsRequired();
                entity.Property(e => e.HasSajda).IsRequired();
                entity.Property(e => e.SajdaId).IsRequired(false);
               entity.Property(e=>e.StopVerse).IsRequired();
                // Index for querying sajda verses
                entity.HasIndex(e => e.HasSajda);
            });

            // AudioRecitation Configuration
            modelBuilder.Entity<AudioRecitation>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ReciterNameAr).IsRequired().HasMaxLength(200);
                entity.Property(e => e.ReciterNameEn).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Link).IsRequired();
            });
        }
    }
}
