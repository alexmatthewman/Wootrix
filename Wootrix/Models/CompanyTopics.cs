using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WootrixV2.Models
{
    public class CompanyTopics
    { 

        public int ID { get; set; }

        [ScaffoldColumn(false)]
        public int CompanyID { get; set; }

        [Required]
        [RegularExpression(@"^[^\|]+$", ErrorMessage = "Please no | characters")]
        [StringLength(1000)]
        [Display(Name = "Topics", Prompt = "Topics", Description = "Company Topics")]
        public string Topic { get; set; }
        
    }
}
