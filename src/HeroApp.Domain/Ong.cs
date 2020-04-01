
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeroApp.Domain
{
    public class Ong
    {
        public Ong()
        {
            Incidents = new HashSet<Incident>();
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string WhatsApp { get; set; }
        public string City { get; set; }
        public string State { get; set; }


        public virtual ICollection<Incident> Incidents { get; set; }

        public long AppUserId { get; set; }
        public AppUser Owner { get; set; }

       
    }
}
