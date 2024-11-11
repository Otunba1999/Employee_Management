using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLibrary.DTOs;

namespace ClientLibrary.Helpers
{
    public class HttpClientService(IHttpClientFactory httpClientFactory, LocalStorageService localStorageService)
    {
        private const string HeaderKey = "Authorization";
        public async Task<HttpClient> GetPrivateHttpClient(){
            var client = httpClientFactory.CreateClient("SystemAPIClient");
            var token = await localStorageService.GetToken();
            if(string.IsNullOrEmpty(token)) return client;
            var deserilizeToken = Serilization.DeserializeObj<UserSession>(token);
            if(deserilizeToken == null) return client;
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", deserilizeToken.Token);
            return client;
        }
        public HttpClient GetHttpClient(){
            var client = httpClientFactory.CreateClient("SystemAPIClient");
            client.DefaultRequestHeaders.Remove(HeaderKey);
            return client;
        }
    }
}