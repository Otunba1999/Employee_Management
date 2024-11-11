using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaseLibrary.Entities
{
    public class OverTimeType : BaseEntity
    {
        // one to many relationship with OverTime
        public List<OverTime>? OverTimes { get; set; }
    }
}