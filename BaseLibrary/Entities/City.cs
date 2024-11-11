using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BaseLibrary.Entities
{
    public class City : BaseEntity
    {
        // many to one relationship with country
        public Country? Country {get; set;}
        public int CountryId { get; set; }
        // one to many relationship with Town
         [JsonIgnore]
        public List<Town>? Towns { get; set; }
    }
}