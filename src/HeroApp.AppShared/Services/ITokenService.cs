using HeroApp.AppShared.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HeroApp.AppShared.Services
{
    public interface ITokenService
    {
        Task Clear();
        Task<string> GetToken();
      
        Task StoreToken(LocalUserInfo localUserInfo);
    }
}
