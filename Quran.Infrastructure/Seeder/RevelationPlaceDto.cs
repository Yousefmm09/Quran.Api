using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Quran.Infrastructure.Seeder
{
    public class RevelationPlaceDto
    {
        [JsonPropertyName("ar")]
        public string Ar { get; set; }

        [JsonPropertyName("en")]
        public string En { get; set; }
    }
}
