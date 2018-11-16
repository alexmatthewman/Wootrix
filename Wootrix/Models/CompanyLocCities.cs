using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WootrixV2.Models
{
    public class CompanyLocCities
    {
        [StringLength(2)]
        public string country_code { get; set; }

        [StringLength(50)]
        public string state_code { get; set; }

        [Key]
        [Display(Name = "City", Prompt = "Select the City name", Description = "City Name")]
        [StringLength(500)]
        public string city_name_ascii { get; set; }
    }
}
