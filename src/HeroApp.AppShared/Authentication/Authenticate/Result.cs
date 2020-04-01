using System;
using System.Collections.Generic;
using System.Text;

namespace HeroApp.AppShared.Authentication.Authenticate
{
    public class Result
    {
        // public string AcToken { get; set; }
        public string Expiration { get;  set; }
        public string AccessToken { get; set; }
        public string Message { get;  set; }
        public bool Authenticated { get;  set; }
        public string Created { get;  set; }
        public string UserName { get; set; }
    }
}
