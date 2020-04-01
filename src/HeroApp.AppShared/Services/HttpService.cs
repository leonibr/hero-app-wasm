
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace HeroApp.AppShared.Services
{

    public class HttpService : IHttpService
    {

        private readonly string baseUrl;
        private readonly HttpClient client;
        private readonly ITokenService tokenService;

        public HttpService(IBaseUrlService baseUrlService,
            ITokenService tokenService)
        {

            baseUrl = baseUrlService.Uri.ToString();
            client = new HttpClient()
            {
                BaseAddress = new Uri(baseUrl)
            };
            this.tokenService = tokenService;
        }

        HttpContent CreateJsonBody(object model)
        {
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
        public async Task<T> Get<T>(params string[] path) where T : class
        {
            var url = path.ToUrl();
            await SetAuthHeader(client);

            var response = await client.GetAsync(url);


            return await response.ToObject<T>();


        }

        public async Task<T> Post<T>(object model, params string[] path) where T : class
        {

            var url = path.ToUrl();
            await SetAuthHeader(client);

            var content = CreateJsonBody(model);


            // Make a request 
            var response = await client.PostAsync(url, content);

            return await response.ToObject<T>();
        }

        public async Task<T> Put<T>(object model, params string[] path) where T : class
        {
            var url = path.ToUrl();
            await SetAuthHeader(client);

            var content = CreateJsonBody(model);


            // Make a request 
            var response = await client.PutAsync(url, content);

            return await response.ToObject<T>();
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
                data = JsonSerializer.Deserialize<T>(await response.Content.ReadAsStringAsync(), options: jsonSerializerOptions);

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
