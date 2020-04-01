using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace HeroApp.Domain
{
    public class AppUser : IdentityUser
    {
        // public new long Id { get; set; }
        public virtual ICollection<Ong> MyOngs { get; set; }
    }


}
