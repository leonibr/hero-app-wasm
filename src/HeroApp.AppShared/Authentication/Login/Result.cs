using System;
using System.Collections.Generic;
using System.Text;

namespace HeroApp.AppShared.Authentication.Login
{
    public class Result
    {
        // public string AcToken { get; set; }

        public string AccessToken { get; set; }
        public string Message { get;  set; }
        public bool Authenticated { get;  set; }

        public string UserId { get; set; }
    }
}
