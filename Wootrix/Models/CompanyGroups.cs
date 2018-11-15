using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WootrixV2.Models
{
    public class CompanyGroups 
    { 

        public int ID { get; set; }

        [ScaffoldColumn(false)]
        public int CompanyID { get; set; }

        [Required]
        [StringLength(1000)]
        [Display(Name = "Group Name", Prompt = "Enter the Group name", Description = "Group Name")]
        public string GroupName { get; set; }
        
    }
}
