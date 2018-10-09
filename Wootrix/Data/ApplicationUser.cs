using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using WootrixV2.Models;

namespace WootrixV2.Data
{

    public enum Gender
    {
        Male, Female, Other
    }

    public enum Roles
    {
        Admin, CompanyAdmin, User
    }

    public class ApplicationUser : IdentityUser
    {
        public virtual string name { get; set; }
        public virtual string companyName { get; set; }
        public virtual int companyID { get; set; }
        public virtual Gender? gender { get; set; }

        public virtual string categories { get; set; }
        public virtual string photoUrl { get; set; }

        public virtual string registrationType { get; set; }
        public virtual string linkedInID { get; set; }
        public virtual string facebookID { get; set; }
        public virtual string twitterID { get; set; }
        public virtual string googleID { get; set; }
        public virtual string socialAccountToken { get; set; }
        public virtual string token { get; set; }

        public virtual string location { get; set; }
        public virtual string latitude { get; set; }
        public virtual string logitude { get; set; }

        public virtual string articleLanguageID { get; set; }
        public virtual string websiteLanguage { get; set; }
        public virtual string websiteLanguageID { get; set; }

        public virtual string deviceType { get; set; }
        public virtual string osType { get; set; }
        public virtual string deviceIosID { get; set; }
        public virtual string deviceAndroidID { get; set; }
        public virtual string deviceWebID { get; set; }
    }

    public class User
    {
        [ScaffoldColumn(false)]
        public virtual int ID { get; set; }

        [Display(Name = "Email address", Prompt = "Email address", Description = "Email")]
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public virtual string emailAddress { get; set; }

        
        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$", ErrorMessage = "Please only enter a string")]
        [StringLength(1000)]
        [Display(Name = "Full Name", Prompt = "Enter your full name", Description = "Full Name")]
        public virtual string name { get; set; }

        [Required]
        [Display(Name = "Role", Prompt = "Role", Description = "Role")]
        public virtual Roles role { get; set; }

        [Display(Name = "Phone Number", Prompt = "Phone Number", Description = "Phone Number")]
        public virtual int phoneNumber { get; set; }

        [ScaffoldColumn(false)]
        public virtual string companyName { get; set; }

        [ScaffoldColumn(false)]
        public virtual string companyID { get; set; }

        [Display(Name = "Gender", Prompt = "Gender", Description = "Gender")]
        public virtual Gender? gender { get; set; }

        [Display(Name = "Department", Prompt = "Department", Description = "Department")]
        public virtual CompanyDepartments categories { get; set; } // I'll use this to store the Department the user is in - NB ONLY FOR COMPANY ADMINS
        
        [Display(Name = "Avatar Photo", Prompt = "Avatar Photo", Description = "Avatar Photo")]
        public virtual IFormFile photoUrl { get; set; }

        [ScaffoldColumn(false)]
        public virtual string registrationType { get; set; }

        [ScaffoldColumn(false)]
        public virtual string token { get; set; } //redundant

        [Display(Name = "Country", Prompt = "Country", Description = "Country")]
        public virtual CompanyLocCountries country { get; set; }

        [Display(Name = "State", Prompt = "State", Description = "State")]
        public virtual CompanyLocStates state { get; set; }

        [Display(Name = "City", Prompt = "City", Description = "City")]
        public virtual CompanyLocCities city { get; set; }

        public virtual string latitude { get;  } //not being used
        public virtual string logitude { get;  } //not being used

        [ScaffoldColumn(false)]
        public virtual string articleLanguageID { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$", ErrorMessage = "Please only enter a string")]
        [StringLength(100)]
        [Display(Name = "Website Language", Prompt = "Website Language", Description = "Website Language")]
        public virtual string websiteLanguage { get; set; }

        [ScaffoldColumn(false)]
        public virtual string websiteLanguageID { get; set; }

        public virtual string deviceType { get;  }
        public virtual string osType { get;  }
        public virtual string deviceIosID { get;  }
        public virtual string deviceAndroidID { get;  }
        public virtual string deviceWebID { get;  }

        public virtual string modifiedBy { get;  }
        public virtual string createdBy { get;  }
        public virtual DateTime moditfiedOn { get;  }
        public virtual DateTime createdOn { get;  }

    }
}

  
