using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Server.Repositories.Interfaces;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenericController<T>(IGenericRepository<T> genericRepository) : Controller where T : class
    {
        [HttpGet("all")]
        public async Task<IActionResult> GetAll() => Ok(await genericRepository.GetAll());
        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id){
            if(id <= 0) return BadRequest("Invalid request sent");
            return Ok(await genericRepository.DeleteById(id));
        }
        [HttpGet("single/{id:int}")]
        public async Task<IActionResult> GetSingle(int id){
            if(id <= 0) return BadRequest("Invalid request sent");
            return Ok(await genericRepository.GetById(id));
        }
        [HttpPost("add")]
        public async Task<IActionResult> Add(T entity){
            if(entity == null) return BadRequest("Invalid request sent");
            return Ok(await genericRepository.Insert(entity));
        }
        [HttpPut("update")]
        public async Task<IActionResult> Update(T entity){
            if(entity == null) return BadRequest("Invalid request sent");
            return Ok(await genericRepository.Update(entity));
        }
    }
}