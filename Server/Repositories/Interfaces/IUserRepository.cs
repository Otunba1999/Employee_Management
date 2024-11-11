using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLibrary.DTOs;
using BaseLibrary.Entities;
using BaseLibrary.Responses;

namespace Server.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<GeneralResponse> RegisterAsync(Register register);
        Task<LoginResponse> SignInAsync(Login login);
        Task<LoginResponse> GetRefreshTokenAsync(RefreshTokenDto refreshToken);
         Task<List<ManageUser>> GetUsers();
        Task<GeneralResponse> UpdateUser(ManageUser user);
        Task<List<SystemRole>> GetRoles();
        Task<GeneralResponse> DeleteUser(int id);
    }
}