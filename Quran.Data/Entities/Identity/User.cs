using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quran.Data.Entities.Identity
{
    public class User: IdentityUser
    {
        public int Id { get; set; }   // Clustered PK (Fast)

        public Guid PublicId { get; set; } = Guid.NewGuid();

        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;

    }
}
