using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quran.Infrastructure.Context;

namespace Infrastructure.Data
{
    public static class QuranSeederExtensions
    {
        public static async Task SeedQuranData(this IServiceProvider serviceProvider, string jsonFilePath)
        {
            using var scope = serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<AppDb>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<QuranSeeder>>();

            var seeder = new QuranSeeder(context, logger);
            await seeder.SeedTextArabicSearchFromJson(jsonFilePath);
        }
    }
}