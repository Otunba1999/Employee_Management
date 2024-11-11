using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BaseLibrary.DTOs
{
    public class EmployeeGroup2
    {
        [Required]
        public string JobName {get; set;} = string.Empty;
        [Required, Range(1,9999, ErrorMessage = "You must select a branch")]
        public int BranchId {get; set;}
        [Required, Range(1,9999, ErrorMessage = "You must select a town")]
        public int TownId {get; set;}
        public string? Other {get; set;}
    }
}