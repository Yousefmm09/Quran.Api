using Microsoft.Extensions.DependencyInjection;
using Quran.Infrastructure.Abstract;
using Quran.Infrastructure.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quran.Infrastructure
{
    public static class ModuleInfrastructureDependencies
    {
       public static IServiceCollection GetService(this IServiceCollection services)
        { 
            services.AddTransient<ISurahRepo, SurahRepo>();
            services.AddTransient<IVersesRepo, Verses>();
            services.AddTransient<IAudioRepo, AudioRepo>();
            return services;
        }
    }
}
