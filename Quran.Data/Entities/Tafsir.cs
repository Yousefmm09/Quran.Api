using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quran.Data.Entities
{
    public class Tafsir
    {
        public int Id { get; set; }
        public int AyahId { get; set; }
        public string Text { get; set; }

        public Verse Ayah { get; set; }
    }
}
