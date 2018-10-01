using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WootrixV2.Models
{
    public class CompanyLocations
    {
        [Key]
        public int ID { get; set; }

        public int CompanyID { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$", ErrorMessage = "Please only enter a string")]
        [StringLength(1000)]
        [Display(Name = "Location Name", Prompt = "Enter the Location name", Description = "Location Name")]
        public string LocationName { get; set; }

        

        //Might have to have these pull from a list but for now just entry should suffice


    }
}
