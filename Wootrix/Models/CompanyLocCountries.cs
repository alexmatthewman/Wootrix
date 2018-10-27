using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WootrixV2.Models
{
    public class CompanyLocCountries
    {

        //[ScaffoldColumn(false)]
        //public int ID { get; set; }

        //[ScaffoldColumn(false)]
        //public int CompanyID { get; set; }

        [Key]
        [StringLength(2)]
        public string country_code { get; set; }

        [Display(Name = "Country Name", Prompt = "Enter the Country name", Description = "Country Name")]
        [StringLength(500)]
        public string country_name { get; set; }

    }
}
