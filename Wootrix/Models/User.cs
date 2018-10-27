using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WootrixV2.Models
{
    public enum Gender
    {
        Male, Female, Other
    }

    public enum Roles
    {
        Admin, CompanyAdmin, User
    }

    public class User
    {
        [ScaffoldColumn(false)]
        public int ID { get; set; }

        [ScaffoldColumn(false)]
        public string GuidID { get; set; }

        [ScaffoldColumn(false)]
        public int CompanyID { get; set; }

        [ScaffoldColumn(false)]
        public string CompanyName { get; set; }

        [Display(Name = "Email address", Prompt = "Email address", Description = "Email")]
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string EmailAddress { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$", ErrorMessage = "Please only enter a string")]
        [StringLength(1000)]
        [Display(Name = "Full Name", Prompt = "Enter your full name", Description = "Full Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Role", Prompt = "Role", Description = "Role")]
        public Roles Role { get; set; }

        [Display(Name = "Phone Number", Prompt = "Phone Number", Description = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Gender", Prompt = "Gender", Description = "Gender")]
        public string Gender { get; set; }

        [Display(Name = "Avatar Photo", Prompt = "Avatar Photo", Description = "Avatar Photo")]
        public string Photo { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$", ErrorMessage = "Please only enter a string")]
        [StringLength(100)]
        [Display(Name = "Website Language", Prompt = "Website Language", Description = "Website Language")]
        public string WebsiteLanguage { get; set; }

        [ScaffoldColumn(false)]
        public string WebsiteLanguageID { get; set; }

        [Display(Name = "Department", Prompt = "Department", Description = "Department")]
        public string Categories { get; set; } // I'll use this to store the Department the user is in - NB ONLY FOR COMPANY ADMINS

        [Display(Name = "Topics", Prompt = "Topics", Description = "Topics")]
        public string Topics { get; set; }

        [Display(Name = "Groups", Prompt = "Groups", Description = "Groups")]
        public string Groups { get; set; }

        [Display(Name = "User Type", Prompt = "User Type", Description = "User Type")]
        public string TypeOfUser { get; set; }

        [Display(Name = "Country", Prompt = "Country", Description = "Country")]
        public string Country { get; set; }

        [Display(Name = "State", Prompt = "State", Description = "State")]
        public string State { get; set; }

        [Display(Name = "City", Prompt = "City", Description = "City")]
        public string City { get; set; }

        [ScaffoldColumn(false)]
        public string RegistrationType { get; set; }

        [ScaffoldColumn(false)]
        public string Token { get; set; } //redundant

        [ScaffoldColumn(false)]
        public string ArticleLanguageID { get; set; }

        [ScaffoldColumn(false)]
        public string Latitude { get; } //not being used
        [ScaffoldColumn(false)]
        public string Logitude { get; } //not being used

        [ScaffoldColumn(false)]
        public string DeviceType { get; set; }

        [ScaffoldColumn(false)]
        public string OsType { get; set; }

        [ScaffoldColumn(false)]
        public string DeviceIosID { get; set; }

        [ScaffoldColumn(false)]
        public string DeviceAndroidID { get; set; }

        [ScaffoldColumn(false)]
        public string DeviceWebID { get; set; }

        [ScaffoldColumn(false)]
        public string ModifiedBy { get; set; }

        [ScaffoldColumn(false)]
        public string CreatedBy { get; set; }

        [ScaffoldColumn(false)]
        public DateTime ModitfiedOn { get; set; }

        [ScaffoldColumn(false)]
        public DateTime CreatedOn { get; set; }
    }




    public class UserViewModel
    {
        [ScaffoldColumn(false)]
        public int ID { get; set; }

        [ScaffoldColumn(false)]
        public int CompanyID { get; set; }

        [ScaffoldColumn(false)]
        public string CompanyName { get; set; }

        [Display(Name = "Email address", Prompt = "Email address", Description = "Email")]
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string EmailAddress { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [StringLength(1000)]
        [Display(Name = "Full Name", Prompt = "Enter your full name", Description = "Full Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Role", Prompt = "Role", Description = "Role")]
        public Roles Role { get; set; }

        //[DataType(DataType.PhoneNumber)]
        //[StringLength(15, ErrorMessage = "The {0} must be at max {1} characters long.")]
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        [Display(Name = "Phone Number", Prompt = "Phone Number", Description = "Phone Number")]
        public string PhoneNumber { get; set; }


        [Display(Name = "Gender", Prompt = "Gender", Description = "Gender")]
        public string Gender { get; set; }
        public IEnumerable<SelectListItem> Genders { get; set; }
  
        [Display(Name = "Avatar Photo", Prompt = "Avatar Photo", Description = "Avatar Photo")]
        public IFormFile Photo { get; set; }

        
        
        [Display(Name = "Languages")]
        public string WebsiteLanguage { get; set; }
        public IList<string> SelectedLanguages { get; set; }
        public IList<SelectListItem> AvailableLanguages { get; set; }

        [ScaffoldColumn(false)]
        public string WebsiteLanguageID { get; set; }

        [Display(Name = "Limit editors to this Department", Prompt = "Limit editors to this Department", Description = "Limit editors to this Department")]
        public string Categories { get; set; }
        public IEnumerable<SelectListItem> Departments { get; set; }

        [Display(Name = "User groups", Prompt = "User groups", Description = "User groups")]
        public string Groups { get; set; }
        public IList<string> SelectedGroups { get; set; }
        public IList<SelectListItem> AvailableGroups { get; set; }

        [Display(Name = "User Topics", Prompt = "User Topics", Description = "User Topics")]
        public string Topics { get; set; }
        public IList<string> SelectedTopics { get; set; }
        public IList<SelectListItem> AvailableTopics { get; set; }

        [Display(Name = "Type Of User", Prompt = "Type Of User", Description = "Type Of User")]
        public string TypeOfUser { get; set; }
        public IList<string> SelectedTypeOfUser { get; set; }
        public IList<SelectListItem> AvailableTypeOfUser { get; set; }

        [Display(Name = "User Country", Prompt = "User Country", Description = "User Country")]
        public string Country { get; set; }
        public IEnumerable<SelectListItem> Countries { get; set; }

       
        [Display(Name = "State", Prompt = "State", Description = "State")]
        public string State { get; set; }
        public IEnumerable<SelectListItem> States { get; set; }

        [Display(Name = "City", Prompt = "City", Description = "City")]
        public string City { get; set; }
        public IEnumerable<SelectListItem> Cities { get; set; }

        public UserViewModel()
        {
            SelectedGroups = new List<string>();
            AvailableGroups = new List<SelectListItem>();

            SelectedTopics = new List<string>();
            AvailableTopics = new List<SelectListItem>();

            SelectedTypeOfUser = new List<string>();
            AvailableTypeOfUser = new List<SelectListItem>();

            SelectedLanguages = new List<string>();
            AvailableLanguages = new List<SelectListItem>();
            
            Countries = new List<SelectListItem>();
            States = new List<SelectListItem>();
            Cities = new List<SelectListItem>();

        }
    }
}