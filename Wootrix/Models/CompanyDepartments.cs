using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WootrixV2.Models
{
    public class CompanyDepartments
    { 
        public int ID { get; set; }

        [ScaffoldColumn(false)]
        public int CompanyID { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$", ErrorMessage = "Please only enter a string")]
        [StringLength(1000)]
        [Display(Name = "Department", Prompt = "Department Name", Description = "Department Name ")]
        public string CompanyDepartmentName { get; set; }


        //This is a filter for Segments/Magazines - so a company can specify departments, then if a Company Admin user is added with a 
        //department then they can only see Segments/Magazines from their department

    }
}
