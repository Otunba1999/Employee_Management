using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BaseLibrary.DTOs;
using Microsoft.AspNetCore.Components.Authorization;

namespace ClientLibrary.Helpers
{
    public class CustomAuthStateProvider(LocalStorageService localStorageService) : AuthenticationStateProvider
    {
        private readonly ClaimsPrincipal annonymous = new ClaimsPrincipal(new ClaimsIdentity());
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await localStorageService.GetToken();
            if (string.IsNullOrEmpty(token))
                return await Task.FromResult(new AuthenticationState(annonymous));
            var deserializeToken = Serilization.DeserializeObj<UserSession>(token);
            if (deserializeToken == null) return await Task.FromResult(new AuthenticationState(annonymous));
            var userClaims = DecryptToken(deserializeToken.Token);
            if (userClaims == null) return await Task.FromResult(new AuthenticationState(annonymous));
            var claimPrincipal = SetClaimPrincipal(userClaims);
            return await Task.FromResult(new AuthenticationState(claimPrincipal));

        }

        public async Task UpdateAuthenticationState(UserSession userSession)
        {
            var claimPrincipal = new ClaimsPrincipal();
            if (userSession.Token is not null || userSession.RefreshToken is not null)
            {
                var serializeSession = Serilization.SerializeObj(userSession);
                await localStorageService.SetToken(serializeSession);
                var userClaims = DecryptToken(userSession.Token);
                claimPrincipal = SetClaimPrincipal(userClaims);
            }
            else
            {
                await localStorageService.RemoveToken();
            }
            var authState = Task.FromResult(new AuthenticationState(claimPrincipal));
            NotifyAuthenticationStateChanged(authState);
        }

        private static ClaimsPrincipal SetClaimPrincipal(CustomUserClaim userClaims)
        {
            if (userClaims is null) return new ClaimsPrincipal();
            return new ClaimsPrincipal(new ClaimsIdentity(new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, userClaims.Id!),
                new Claim(ClaimTypes.Name, userClaims.Name!),
                new Claim(ClaimTypes.Email, userClaims.Email!),
                new Claim(ClaimTypes.Role, userClaims.Role!)
            }, "JwtAuth"));
        }

        private static CustomUserClaim DecryptToken(string token)
        {
            if (string.IsNullOrEmpty(token)) return new CustomUserClaim();
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            var name = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            var email = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            var roles = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
            return new CustomUserClaim
            {
                Id = userId!.Value,
                Name = name!.Value,
                Email = email!.Value,
                Role = roles!.Value
            };
        }
    }
}