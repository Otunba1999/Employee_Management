using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BaseLibrary.Entities
{
    public class GenaralDepartement : BaseEntity
    {
        // one to many relationship with department
         [JsonIgnore]
        public List<Department>? Departments { get; set; }
    }
}