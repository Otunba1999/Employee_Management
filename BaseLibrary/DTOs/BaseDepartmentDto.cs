using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BaseLibrary.DTOs
{
    public class BaseDepartmentDto
    {
        public int Id { get; set; }
        [Required, MaxLength(50), MinLength(3)]
        public string? Name { get; set; }
    }
}