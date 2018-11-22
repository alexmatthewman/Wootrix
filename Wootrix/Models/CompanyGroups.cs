using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WootrixV2.Models
{
    public class CompanyGroups 
    {
        private const string V = @"^[^\|]+$";

        public int ID { get; set; }

        [ScaffoldColumn(false)]
        public int CompanyID { get; set; }

        [Required]
        [RegularExpression(V, ErrorMessage = "Please no | characters")]
        [StringLength(1000)]
        [Display(Name = "Groups", Prompt = "Enter the Group name", Description = "Group Name")]
        public string GroupName { get; set; }
        
    }
}
