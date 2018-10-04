using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WootrixV2.Models
{
    public class CompanyLocCities
    {
        [Key]
        public int ID { get; set; }

        public int CompanyLocStatesID { get; set; }

        public int CompanyID { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$", ErrorMessage = "Please only enter a string")]
        [StringLength(1000)]
        [Display(Name = "City Name", Prompt = "Enter the City name", Description = "City Name")]
        public string CityName { get; set; }

        

        //Might have to have these pull from a list but for now just entry should suffice


    }
}
