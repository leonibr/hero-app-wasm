using HeroApp.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace HeroApp.App.Services
{
    public class GenerateIdService : IGenerateIdService
    {
        private static RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();
        public string GenerateId()
        {
            byte[] bytes = new byte[4];
            rngCsp.GetBytes(bytes);
            rngCsp.Dispose();
            var response = bytes
                .Select(c => c.ToString("X"))
                .Aggregate( (a, p) => $"{a}{p}");
            return response;
        }
    }
}
