using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BaseLibrary.DTOs;
using BaseLibrary.Entities;
using BaseLibrary.Responses;
using ClientLibrary.Helpers;
using ClientLibrary.Services.Interfaces;

namespace ClientLibrary.Services.Implementations
{
    public class UserService(HttpClientService httpClientService) : IUserService
    {
        private const string AuthUrl = "api/auth";
        public async Task<GeneralResponse> CreateAsync(Register register)
        {
           var httpClient = httpClientService.GetHttpClient();
           var result = await httpClient.PostAsJsonAsync($"{AuthUrl}/register", register);
           if(!result.IsSuccessStatusCode) return new GeneralResponse(false, "An error occured");
           return await result.Content.ReadFromJsonAsync<GeneralResponse>();
        }

        public async Task<GeneralResponse> DeleteUser(int id)
        {
            var httpClient = httpClientService.GetHttpClient();
            var result = await httpClient.DeleteAsync($"{AuthUrl}/delete-user/{id}");
            if(!result.IsSuccessStatusCode) return new GeneralResponse(false, "An error occured");
            var response = await result.Content.ReadFromJsonAsync<GeneralResponse>();
            return response!;

        }

        public async Task<List<SystemRole>> GetRoles()
        {
            var httpClient = httpClientService.GetHttpClient();
            var result = await httpClient.GetFromJsonAsync<List<SystemRole>>($"{AuthUrl}/roles");
            return result!;
        }

        public async Task<List<ManageUser>> GetUsers()
        {
            var httpClient = await httpClientService.GetPrivateHttpClient();
            var result = await httpClient.GetFromJsonAsync<List<ManageUser>>($"{AuthUrl}/users");
            return result!;
        }

        public async Task<LoginResponse> LoginAsync(Login login)
        {
            var httpClient = httpClientService.GetHttpClient();
            var result = await httpClient.PostAsJsonAsync($"{AuthUrl}/login", login);
            if(!result.IsSuccessStatusCode) return new LoginResponse(false, "An error occured");
            return await result.Content.ReadFromJsonAsync<LoginResponse>();
        }

        public async Task<LoginResponse> RefreshTokenAsync(RefreshTokenDto refreshToken)
        {
            var httpClient = httpClientService.GetHttpClient();
            var result = await httpClient.PostAsJsonAsync($"{AuthUrl}/refresh-token", refreshToken);
            if(!result.IsSuccessStatusCode) return new LoginResponse(false, "An error occured");
            return await result.Content.ReadFromJsonAsync<LoginResponse>();
        }

        public async Task<GeneralResponse> UpdateUser(ManageUser user)
        {
            var httpClient = httpClientService.GetHttpClient();
            var result = await httpClient.PutAsJsonAsync($"{AuthUrl}/update-user", user);
            if(!result.IsSuccessStatusCode) return new GeneralResponse(false, "An error occured");
            return await result.Content.ReadFromJsonAsync<GeneralResponse>();
        }
    }
}