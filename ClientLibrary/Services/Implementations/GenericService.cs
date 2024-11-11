using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BaseLibrary.Responses;
using ClientLibrary.Helpers;
using ClientLibrary.Services.Interfaces;

namespace ClientLibrary.Services.Implementations
{
    public class GenericService<T>(HttpClientService httpClientService) : IGenericService<T>
    {
        public async Task<GeneralResponse> Delete(string baseUrl, int id)
        {
           var httpClient = await GetHttpClient(httpClientService);
           var response = await httpClient.DeleteAsync($"{baseUrl}/delete/{id}");
           var result = await response.Content.ReadFromJsonAsync<GeneralResponse>();
           return result!;
        }

        public async Task<T> Get(string baseUrl, int id)
        {
            var httpClient = await httpClientService.GetPrivateHttpClient();
            var response = await httpClient.GetFromJsonAsync<T>($"{baseUrl}/single/{id}");
            return response!;
        }

        public async Task<List<T>> GetAll(string baseUrl)
        {
            var httpClient = await GetHttpClient(httpClientService);
            var response = await httpClient.GetFromJsonAsync<List<T>>($"{baseUrl}/all");
            return response!;
        }

        public async Task<GeneralResponse> Insert(string baseUrl, T model)
        {
            var httpClient = await GetHttpClient(httpClientService);
            var response = await httpClient.PostAsJsonAsync($"{baseUrl}/add", model);
            var result = await response.Content.ReadFromJsonAsync<GeneralResponse>();
            return result!;

        }

        public async Task<GeneralResponse> Update(string baseUrl, T model)
        {
            var httpClient = await GetHttpClient(httpClientService);
            var response = await httpClient.PutAsJsonAsync($"{baseUrl}/update", model);
            var result = await response.Content.ReadFromJsonAsync<GeneralResponse>();
            return result!;
        }

        private static async Task<HttpClient> GetHttpClient(HttpClientService httpClientService)
        {
            return await httpClientService.GetPrivateHttpClient();
        }
    }
}