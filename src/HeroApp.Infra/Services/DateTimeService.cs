using HeroApp.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HeroApp.Infra.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
