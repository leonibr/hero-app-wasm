using System;
using System.Collections.Generic;
using System.Text;

namespace HeroApp.Domain.Interfaces
{
    public interface IDateTime
    {
        DateTime Now { get; }
    }
}
