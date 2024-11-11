using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using BaseLibrary.DTOs;
using ClientLibrary.Services.Interfaces;

namespace ClientLibrary.Helpers
{
    public class CustomHttpHandler(
        HttpClientService httpClient, LocalStorageService localStorageService, IUserService userService) : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            bool loginUrl = request.RequestUri!.AbsoluteUri.Contains("login");
            bool registerUrl = request.RequestUri!.AbsoluteUri.Contains("register");
            bool refreshTokenUrl = request.RequestUri!.AbsoluteUri.Contains("refresh-token");
            if (loginUrl || registerUrl || refreshTokenUrl) return await base.SendAsync(request, cancellationToken);

            var result = await base.SendAsync(request, cancellationToken);
            if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                // get token from local storage
                var stringToken = await localStorageService.GetToken();
                if (stringToken is null) return result;

                // check if header contains token
                string token = string.Empty;
                try{
                    token = request.Headers.Authorization!.Parameter!;
                } catch (Exception) { }
                var desrializedToken = Serilization.DeserializeObj<UserSession>(stringToken);
                if(desrializedToken is null) return result;
                if(string.IsNullOrEmpty(token)){
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", desrializedToken.Token);
                    return await base.SendAsync(request, cancellationToken);
                }
                // call for refresh token
                var newToken = await GetRefreshToken(desrializedToken.RefreshToken);
                if (string.IsNullOrEmpty(newToken)) return result;
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", newToken);
                return await base.SendAsync(request, cancellationToken);
            }
            return result;
        }

        private async Task<string> GetRefreshToken(string? refreshToken)
        {
            var result = await userService.RefreshTokenAsync(new RefreshTokenDto(){Token = refreshToken});
            if (result is null) return null;
            var serializedToken = Serilization.SerializeObj(new UserSession(){Token = result.Token, RefreshToken = result.RefreshToken});
            await localStorageService.SetToken(serializedToken);
            return result.Token;
        }
    }

}