using Microsoft.Extensions.DependencyInjection;
using Quran.Services.Abstract;
using Quran.Services.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quran.Services
{
    public static class ModuleServiceDependencies
    {
        public static IServiceCollection getService (this IServiceCollection services)
        {
            services.AddTransient<ISurahService, SurahService>();
            services.AddTransient<IVerses, VersesService>();
            return services;
        }
    }
}
