using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace HeroApp.AppShared.Model
{
    public class LocalUserInfo
    {
        public string AccessToken { get; set; }
        public string Id { get; set; }
    }
}
