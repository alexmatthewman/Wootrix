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

}

  
