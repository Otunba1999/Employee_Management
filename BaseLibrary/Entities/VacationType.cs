using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaseLibrary.Entities
{
    public class VacationType : BaseEntity
    {
        // one to many relationship with Vacation
        public List<Vacation>? Vacations { get; set; }
    }
}