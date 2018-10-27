using Microsoft.AspNetCore.Http;
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

        [ScaffoldColumn(false)]
        public int CompanyID { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Language Name", Prompt = "Enter the Language name", Description = "Language Name")]
        public string LanguageName { get; set; }

    }
}
