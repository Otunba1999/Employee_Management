using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLibrary.DTOs;
using BaseLibrary.Entities;
using BaseLibrary.Responses;

namespace ClientLibrary.Services.Interfaces
{
    public interface IUserService
    {
        Task<GeneralResponse> CreateAsync(Register register);
        Task<LoginResponse> LoginAsync(Login login);
        Task<LoginResponse> RefreshTokenAsync(RefreshTokenDto refreshToken);
        Task<List<ManageUser>> GetUsers();
        Task<GeneralResponse> UpdateUser(ManageUser user);
        Task<List<SystemRole>> GetRoles();
        Task<GeneralResponse> DeleteUser(int id);
    }
}