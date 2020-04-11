
using HeroApp.Domain;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Reflection;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace HeroApp.AppShared.Services
{


    public interface IHttpConfigOptions
    {
        int TimeOut { get; set; }
        void AddBeforeRequest<T>();//where T : Type;
        void AddAfterRequest<T>() where T : Type;

        List<Type> AfterRequests { get; }
        List<Type> BeforeRequests { get; }
    }

    public class HttpConfigOptions : IHttpConfigOptions
    {
        public List<Type> AfterRequests { get; }
        public List<Type> BeforeRequests { get; }


        /// <summary>
        /// Http request timeout
        /// </summary>
        public int TimeOut { get; set; } = 10000;

        public void AddBeforeRequest<T>() // where T : Type
        {
            BeforeRequests.Add(typeof(T));
        }

        public void AddAfterRequest<T>() where T : Type
        {
            AfterRequests.Add(typeof(T));
        }


    }

    public static class HttpInterceptorExtensions
    {

        public static void AddHttpClientService(this IServiceCollection services, Action<HttpConfigOptions> action)
        {
            var cfg = new HttpConfigOptions();

            action.Invoke(cfg);

            foreach (var after in cfg.AfterRequests)
            {
                services.TryAddTransient(after);
            }
            foreach (var before in cfg.BeforeRequests)
            {
                services.TryAddTransient(before);
            }

            services.AddSingleton<IHttpConfigOptions>(cfg);
            services.AddScoped<IHttpService, HttpService>();
        }

    }

    public interface IBeforeRequest
    {
        Task Handle(HttpClient client);
    }
    public interface IAfterRequest
    {
        Task<HttpResponseMessage> Handle(HttpResponseMessage responseMessage);
    }
    public class IsMakingRequestEventArgs : EventArgs
    {
        public string Url { get; set; }
        public bool IsLoading { get; set; }
    }
    public enum Method
    {
        GET,
        POST,
        PUT,
        DELETE
    }
    public class HttpService : IHttpService
    {

        private readonly string baseUrl;
        private readonly HttpClient client;
        private readonly IHttpConfigOptions configOptions;
        private readonly IServiceProvider serviceProvider;
        private readonly ITokenService tokenService;
        public event EventHandler<IsMakingRequestEventArgs> IsMakingRequest;

        public HttpService(
            IHttpConfigOptions configOptions,
            IServiceProvider serviceProvider,
            IBaseUrlService baseUrlService,
            ITokenService tokenService
            )
        {
            baseUrl = baseUrlService.Uri.ToString();
            client = new HttpClient()
            {
                BaseAddress = new Uri(baseUrl)
            };
            setCommonHeader(client);
            this.configOptions = configOptions;
            this.serviceProvider = serviceProvider;
            this.tokenService = tokenService;


        }

        private void loadInterceptors(Assembly assembly)
        {
            var typeBefore = typeof(IBeforeRequest);
            var typeAfter = typeof(IAfterRequest);

            //var types = AppDomain.CurrentDomain.GetAssemblies()
            //    .SelectMany(s => s.GetTypes())
            //    .Where(p => typeBefore.IsAssignableFrom(p) || typeAfter.IsAssignableFrom(p) );
            Console.WriteLine("GetExecutingAssembly: " + assembly?.GetName());

            var both = assembly.GetTypes().Where(c =>

                (typeBefore.IsAssignableFrom(c) || typeAfter.IsAssignableFrom(c))
                && !c.IsInterface && c.IsClass
                ).ToList();

            Console.WriteLine("Both : " + both.Count);

            var before = both.Where(c => typeBefore.IsAssignableFrom(c)).ToList();
            Console.WriteLine("before : " + before.Count);

            var after = both.Where(c => typeAfter.IsAssignableFrom(c)).ToList();
            Console.WriteLine("after : " + after.Count);


        }

        private HttpContent CreateJsonBody(object model)
        {
            if (model is null) return null;
            string jsonData = model.ToJson();
            HttpContent content = new StringContent(jsonData);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return content;
        }


        async Task SetAuthHeader(HttpClient httpClient)
        {
            var token = await tokenService.GetToken();
            if (token != null && token.Length > 10)
            {
                AuthenticationHeaderValue authHeader = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Authorization = authHeader;

            }
        }
        private void setCommonHeader(HttpClient httpClient)
        {
            client.DefaultRequestHeaders.Add("Access-Control-Max-Age", "3600");
        }
        public async Task<TResponse> Get<TResponse>(params string[] path) where TResponse : class
        {
            var url = path.ToUrl();
            await SetAuthHeader(client);
            var response = await client.GetAsync(url);


            return await response.ToObject<TResponse>();


        }


        private async Task<TResponse> SendRequest<TRequest, TResponse>(Method method, TRequest model = null, params string[] path) where TRequest : class where TResponse : class
        {

            foreach (var before in configOptions.BeforeRequests)
            {
                var beforeRequest = (IBeforeRequest)serviceProvider.GetService(before);
                await beforeRequest.Handle(client);
            }

            HttpResponseMessage response = null;
            HttpRequestMessage request = null;
            switch (method)
            {
                case Method.GET:
                    request = new HttpRequestMessage(HttpMethod.Get, path.ToUrl());
                    response = await client.SendAsync(request);
                    break;
                case Method.POST:
                    request = new HttpRequestMessage(HttpMethod.Post, path.ToUrl())
                    {
                        Content = CreateJsonBody(model)
                    };
                    response = await client.SendAsync(request);
                    break;
                case Method.PUT:
                    break;
                case Method.DELETE:
                    break;
            }

            if (response is null) throw new NullReferenceException("There is a missing response to process");

            foreach (var after in configOptions.BeforeRequests)
            {
                var afterRequest = (IAfterRequest)serviceProvider.GetService(after);
                await afterRequest.Handle(response);
            }

            return

        }

        public async Task<TResponse> Post<TRequest, TResponse>(TRequest model, params string[] path) where TRequest : class where TResponse : class
        {

            var url = path.ToUrl();
            await SetAuthHeader(client);

            IsLoading(url, isLoading: true);
            var response = await client.PostAsJsonAsync<TRequest>(url, model);
            IsLoading(url, isLoading: false);

            return await response.ToObject<TResponse>();
        }


        protected virtual void OnLoadingChanged(IsMakingRequestEventArgs e)
        {
            EventHandler<IsMakingRequestEventArgs> handler = IsMakingRequest;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        private void IsLoading(string url, bool isLoading)
        {
            OnLoadingChanged(new IsMakingRequestEventArgs()
            {
                Url = url,
                IsLoading = isLoading,
            });
        }

        public async Task<TResponse> Put<TRequest, TResponse>(TRequest model, params string[] path) where TResponse : class where TRequest : class
        {
            var url = path.ToUrl();
            await SetAuthHeader(client);


            var response = await client.PutAsJsonAsync<TRequest>(url, model);

            return await response.ToObject<TResponse>();
        }
        public async Task<T> Delete<T>(params string[] path) where T : class
        {
            var url = path.ToUrl();
            await SetAuthHeader(client);

            var response = await client.DeleteAsync(url);


            return await response.ToObject<T>();
        }




    }

    internal static class JsonExtensions
    {
        private static JsonSerializerOptions jsonSerializerOptions => new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        public static string ToJson(this object data)
        {
            string json = JsonSerializer.Serialize(data, options: jsonSerializerOptions);
            return json;
        }
        public static async Task<T> ToObject<T>(this HttpResponseMessage response) where T : class
        {
            T data = null;
            try
            {

                string body = await response.Content.ReadAsStringAsync();
                data = JsonSerializer.Deserialize<T>(body, options: jsonSerializerOptions);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(data);
                throw ex;
            }
            return data;
        }
        public static string ToUrl(this string[] path)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var segment in path)
            {
                sb.Append(segment);
            }
            return sb.ToString();
        }


    }
}
