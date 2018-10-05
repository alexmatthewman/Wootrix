using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WootrixV2.Models
{
    public class CompanyLocStates
    {
        [Key]
        public int ID { get; set; }

        [ScaffoldColumn(false)]
        public int CompanyLocCountriesID { get; set; }

        [ScaffoldColumn(false)]
        public int CompanyID { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$", ErrorMessage = "Please only enter a string")]
        [StringLength(1000)]
        [Display(Name = "State Name", Prompt = "Enter the State name", Description = "State Name")]
        public string StateName { get; set; }

        

        //Might have to have these pull from a list but for now just entry should suffice


    }
}
