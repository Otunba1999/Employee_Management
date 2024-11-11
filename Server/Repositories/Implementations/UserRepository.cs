using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BaseLibrary.DTOs;
using BaseLibrary.Entities;
using BaseLibrary.Responses;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Server.Data;
using Server.Repositories.Interfaces;
using ServerLibrary.Helper;

namespace Server.Repositories.Implementations
{
    public class UserRepository(IOptions<JwtSection> config, DataContext context) : IUserRepository
    {
        public async Task<GeneralResponse> RegisterAsync(Register register)
        {
            if (register is null) return new GeneralResponse(false, "Invalid data");
            var checkUser = await FindByUserEmail(register.Email);
            if (checkUser is not null) return new GeneralResponse(false, "User already exists");
            // Save user
            var appUser = await AddToDatabase(new ApplicationUser()
            {
                Fullname = register.Fullname,
                Email = register.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(register.Password)
            });
            //Check, create and assign role
            var checkAdminRole = await context.SystemRoles.FirstOrDefaultAsync(_ => _.Name!.Equals(Constants.Admin));
            if (checkAdminRole is null)
            {
                var adminRole = await AddToDatabase(new SystemRole()
                {
                    Name = Constants.Admin
                });
                await AddToDatabase(new UserRole()
                {
                    RoleId = adminRole.Id,
                    UserId = appUser.Id
                });
                return new GeneralResponse(true, "User registered successfully");
            }
            var checkUserRole = await context.SystemRoles.FirstOrDefaultAsync(_ => _.Name!.Equals(Constants.User));
            SystemRole response = new();
            if (checkUserRole is null)
            {
                response = await AddToDatabase(new SystemRole()
                {
                    Name = Constants.User
                });
                await AddToDatabase(new UserRole()
                {
                    RoleId = response.Id,
                    UserId = appUser.Id
                });
                return new GeneralResponse(true, "User registered successfully");
            }
            else
            {
                await AddToDatabase(new UserRole()
                {
                    RoleId = checkUserRole.Id,
                    UserId = appUser.Id
                });
            }
            return new GeneralResponse(true, "User registered successfully");
        }

        public async Task<LoginResponse> SignInAsync(Login login)
        {
            if (login is null) return new LoginResponse(false, "Invalid data");
            var appUser = await FindByUserEmail(login.Email);
            if (appUser is null) return new LoginResponse(false, "User not found");
            //verify password
            var checkPassword = BCrypt.Net.BCrypt.Verify(login.Password, appUser.Password);
            if (!checkPassword) return new LoginResponse(false, "Invalid password");
            var userRole = await FindUserRole(appUser.Id);
            if (userRole is null) return new LoginResponse(false, "Role not found");
            var role = await FindRoleName(userRole.RoleId);
            if (role is null) return new LoginResponse(false, "Role not found");
            var token = GenerateToken(appUser, role!.Name!);
            var refreshToken = GenerateRefreshToken();
            var savedRefreshToken = await context.Tokens.FirstOrDefaultAsync(_ => _.UserId == appUser.Id);
            if (savedRefreshToken is not null)
            {
                savedRefreshToken.Token = refreshToken;
                await context.SaveChangesAsync();
            }
            else
            {
                await AddToDatabase(new RefreshToken()
                {
                    Token = refreshToken,
                    UserId = appUser.Id
                });
            }

            return new LoginResponse(true, "Login successful", token, refreshToken);

        }
        public async Task<LoginResponse> GetRefreshTokenAsync(RefreshTokenDto refreshToken)
        {
            if (refreshToken is null) return new LoginResponse(false, "Invalid Token");
            var token = await context.Tokens.FirstOrDefaultAsync(_ => _.Token!.Equals(refreshToken.Token));
            if (token is null) return new LoginResponse(false, "Invalid Token");
            var user = await context.ApplicationUsers.FirstOrDefaultAsync(_ => _.Id == token.UserId);
            if (user is null) return new LoginResponse(false, "User not found");
            var userRole = await FindUserRole(user.Id);
            var role = await FindRoleName(userRole!.RoleId);
            var newToken = GenerateToken(user, role!.Name!);
            var newRefreshToken = GenerateRefreshToken();

            token.Token = newRefreshToken;
            await context.SaveChangesAsync();
            return new LoginResponse(true, "Login successful", newToken, newRefreshToken);
        }

         public async Task<List<ManageUser>> GetUsers()
        {
            
            var allUsers = await GetAppuser();
            var allRoles = await GetUserRoles();
            var systemRoles = await GetSystemRoles();
            if(allUsers.Count == 0 || allRoles.Count == 0) return null!;
            var users = new List<ManageUser>();
            foreach(var user in allUsers){
                var userRole = allRoles.FirstOrDefault(r => r.UserId == user.Id);
                var roleName =  systemRoles.FirstOrDefault(s => s.Id == userRole!.RoleId);
                users.Add(new ManageUser() {UserId = user.Id, Email = user.Email, Name= user.Fullname, Role = roleName!.Name!});
            }
            return users;
        }
        public async Task<GeneralResponse> UpdateUser(ManageUser user)
        {
           var role = (await GetSystemRoles()).FirstOrDefault(r => r.Name!.Equals(user.Role));
           var userRole = await context.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == user.UserId);
           userRole!.RoleId = role!.Id;
           await context.SaveChangesAsync();
           return new GeneralResponse(true, "User role updated successfully");
        }
        public async Task<List<SystemRole>> GetRoles()
        {
            return await GetSystemRoles();
        }

        public async Task<GeneralResponse> DeleteUser(int id)
        {
            var user = await context.ApplicationUsers.FirstOrDefaultAsync(au => au.Id == id);
            if(user is null) return new GeneralResponse(false, "User not found");
            context.ApplicationUsers.Remove(user);
            await context.SaveChangesAsync();
            return new GeneralResponse(true, "User deleted successfully");
        }


        private async Task<List<SystemRole>> GetSystemRoles()
        {
            return await context.SystemRoles.AsNoTracking().ToListAsync();
        }

        private async Task<List<UserRole>> GetUserRoles()
        {
            return await context.UserRoles.AsNoTracking().ToListAsync();
        }

        private async Task<List<ApplicationUser>> GetAppuser()
        {
            return await context.ApplicationUsers.AsNoTracking().ToListAsync();
        }

        private async Task<T> AddToDatabase<T>(T model)
        {
            var result = context.Add(model!);
            await context.SaveChangesAsync();
            return (T)result.Entity;
        }

        private async Task<ApplicationUser> FindByUserEmail(string? email)
        {
            return await context.ApplicationUsers.FirstOrDefaultAsync(_ => _.Email!.ToLower()!.Equals(email!.ToLower()));
        }
        private string? GenerateToken(ApplicationUser appUser, string role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Value.Key!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var userClaims = new[]{
                new Claim(ClaimTypes.NameIdentifier, appUser.Id.ToString()),
                new Claim(ClaimTypes.Email, appUser.Email!),
                new Claim(ClaimTypes.Name , appUser.Fullname!),
                new Claim(ClaimTypes.Role, role)
            };
            var jwtToken = new JwtSecurityToken(
                issuer: config.Value.Issuer,
                audience: config.Value.Audience,
                claims: userClaims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
        private static string? GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        private async Task<UserRole> FindUserRole(int id) => await context.UserRoles.FirstOrDefaultAsync(_ => _.UserId == id);
        private async Task<SystemRole> FindRoleName(int roleId) => await context.SystemRoles.FirstOrDefaultAsync(_ => _.Id == roleId);

    }
}