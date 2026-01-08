using Microsoft.Extensions.DependencyInjection;
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
            return services;
        }
    }
}
