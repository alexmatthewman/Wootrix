using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

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
        public virtual string companyID { get; set; }
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

    public class UserVM
    {
        // other properties omitted
        public virtual string name { get; set; }
        public virtual string companyName { get; set; }
        public virtual string companyID { get; set; }
        public virtual string gender { get; set; }


        public virtual string categories { get; set; }
        public virtual IFormFile photoUrl { get; set; }

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

        public virtual string modifiedBy { get; set; }
        public virtual string createdBy { get; set; }
        public virtual DateTime moditfiedOn { get; set; }
        public virtual DateTime createdOn { get; set; }

    }
}

  
