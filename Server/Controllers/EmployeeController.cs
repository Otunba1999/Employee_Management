using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLibrary.Entities;
using Microsoft.AspNetCore.Mvc;
using Server.Repositories.Interfaces;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController(IGenericRepository<Employee> employeeRepository)
    : GenericController<Employee>(employeeRepository)
    {

    }
}