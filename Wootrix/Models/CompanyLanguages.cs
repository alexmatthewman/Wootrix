using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WootrixV2.Models
{
    public class CompanyLanguages
    {
        [Key]
        public int ID { get; set; }

        public int CompanyID { get; set; } 

        [Required]
        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$", ErrorMessage = "Please only enter a string")]
        [StringLength(100)]
        [Display(Name = "Language Name", Prompt = "Enter the Language name", Description = "Language Name")]
        public string LanguageName { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$", ErrorMessage = "Please only enter two letter string")]
        [StringLength(2)]
        [Display(Name = "Language Code", Prompt = "Enter the Language two letter code", Description = "Language Code")]
        public string LanguageCode { get; set; }

       
    }
}
