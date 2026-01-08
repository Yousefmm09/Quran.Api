using Microsoft.Extensions.DependencyInjection;
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
            return services;
        }
    }
}
