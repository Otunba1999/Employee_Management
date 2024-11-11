using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLibrary.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Repositories.Interfaces;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AuthController(IUserRepository userRepository) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> CreateAsync(Register register){
            if(!ModelState.IsValid) return BadRequest(ModelState);
            var result = await userRepository.RegisterAsync(register);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(Login login){
            if(!ModelState.IsValid) return BadRequest(ModelState);
            var result = await userRepository.SignInAsync(login);
            return Ok(result);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshTokenAsync(RefreshTokenDto refreshToken){
            if(!ModelState.IsValid) return BadRequest(ModelState);
            var result = await userRepository.GetRefreshTokenAsync(refreshToken);
            return Ok(result);
        }
        [HttpGet("users")]
        [Authorize]
        public async Task<IActionResult> GetUsers(){
            var users = await userRepository.GetUsers();
            if(users is null) return NotFound();
            return Ok(users);
        }
        [HttpPut("update-user")]
        [Authorize]
        public async Task<IActionResult> Update(ManageUser user){
            var response = await userRepository.UpdateUser(user);
            return Ok(response);
        }
        [HttpGet("roles")]
        [Authorize]
        public async Task<IActionResult> GetRoles(){
            var roles = await userRepository.GetRoles();
            if(roles is null) return NotFound();
            return Ok(roles);
        }
        [HttpDelete("delete-user/{id:int}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id){
            var result = await userRepository.DeleteUser(id);
            return Ok(result);
        }
    }
}