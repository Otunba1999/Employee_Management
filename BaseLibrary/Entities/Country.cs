using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BaseLibrary.Entities
{
    public class Country : BaseEntity
    {
        // one to many relationship with cities
         [JsonIgnore]
        public List<City>? Cities { get; set; }
    }
}