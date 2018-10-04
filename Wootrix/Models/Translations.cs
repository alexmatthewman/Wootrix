using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WootrixV2.Models
{
    public class Translations
    { 

        public int ID { get; set; }
        
        [Required]
        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$", ErrorMessage = "Please only enter a string")]
        [StringLength(100)]
        [Display(Name = "Tag Name", Prompt = "Tag Name", Description = "Tag Name")]
        public string TagName { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "English Translation", Prompt = "English Translation", Description = "English Translation")]
        public string EnglishTranslation { get; set; }

        [StringLength(100)]
        [Display(Name = "Spanish Translation", Prompt = "Spanish Translation", Description = "Spanish Translation")]
        public string SpanishTranslation { get; set; }
        
        [StringLength(100)]
        [Display(Name = "Portuguese Translation", Prompt = "Portuguese Translation", Description = "Portuguese Translation")]
        public string PortugueseTranslation { get; set; }



    }
}
