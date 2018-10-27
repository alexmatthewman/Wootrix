using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WootrixV2.Models
{
    public class CompanyLocStates
    {
        [StringLength(2)]
        public string country_code { get; set; }
        
        [Key]
        [StringLength(50)]
        public string state_code { get; set; }

        [Display(Name = "State Name", Prompt = "Enter the State name", Description = "State Name")]
        [StringLength(500)]
        public string state_name { get; set; }
    }
}
