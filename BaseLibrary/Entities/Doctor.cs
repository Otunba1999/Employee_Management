using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BaseLibrary.Entities
{
    public class Doctor : OtherBaseEntity
    {
        [Required]
        public DateTime Date {get; set;}
        [Required]
        public string MedicalDiagonse {get; set;} = string.Empty;
        [Required]
        public string MedicalRecommendation {get; set;} = string.Empty;
    }
}