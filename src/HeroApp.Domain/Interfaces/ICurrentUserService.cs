using System;
using System.Collections.Generic;
using System.Text;

namespace HeroApp.Domain.Interfaces
{
    public interface ICurrentUserService
    {
        string UserId { get; }
    }
}
