using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HeroApp.AppShared.Services
{
    public interface IHttpService
    {
        event EventHandler<IsMakingRequestEventArgs> IsMakingRequest;

        Task<TResponse> Get<TResponse>(params string[] path) where TResponse: class;
        Task<TResponse> Post<TRequest, TResponse>(TRequest model, params string[] path) where TRequest : class where TResponse : class;

        Task<TResponse> Put<TRequest, TResponse>(TRequest model, params string[] path) where TRequest : class where TResponse : class;
        Task<TResponse> Delete<TResponse>(params string[] path) where TResponse : class;

    }
}
