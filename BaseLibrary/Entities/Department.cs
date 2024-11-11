using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BaseLibrary.Entities
{
    public class Department : BaseEntity
    {
        // many to one relationshiop with GeneralDepartment
        public GenaralDepartement? GeneralDepartment { get; set; }
        public int GeneralDepartmentId { get; set; }
        // one to  many relationship with Branch
         [JsonIgnore]
        public List<Branch>? Branches { get; set; }
        
    }
}