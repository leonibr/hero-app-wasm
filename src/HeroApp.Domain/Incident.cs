using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeroApp.Domain
{
    public class Incident
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description{ get; set; }
        public string Value { get; set; }

        public string Ong_Id { get; set; }
        public Ong Ong { get; set; }

    }
}
