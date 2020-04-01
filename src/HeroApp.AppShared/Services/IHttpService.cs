using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HeroApp.AppShared.Services
{
    public interface IHttpService
    {
        Task<T> Get<T>(params string[] path) where T: class;
        Task<T> Post<T>(object model, params string[] path) where T : class;

        Task<T> Put<T>(object model, params string[] path) where T : class;
        Task<T> Delete<T>(params string[] path) where T : class;

    }
}
