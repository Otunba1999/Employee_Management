using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BaseLibrary.Entities
{
    public class Town : BaseEntity
    {
        // one to many relationship with employees
         [JsonIgnore]
        public List<Employee>? Employees { get; set; }

        // one to many relationship with addresses
        // many to one relationship
        public City? City { get; set; }
        public int CityId { get; set; }
    }
}