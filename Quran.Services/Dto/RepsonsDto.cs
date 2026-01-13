using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quran.Services.Dto
{
    public record ApiResponse<T>(
     bool Success,
     string Message,
     T? Data = default,
     object? Errors = null,
     string? TraceId = null
 );

}

